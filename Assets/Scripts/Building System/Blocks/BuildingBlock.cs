using System;
using System.Collections;
using System.Collections.Generic;
using AlertsSystem;
using Building_System.Buildings_Connecting;
using Building_System.Upgrading;
using Events;
using FightSystem.Damage;
using InteractSystem;
using Player_Controller;
using Sound_System;
using Sound_System.FightSystem.Damage;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Blocks
{
    public class BuildingBlock : NetworkBehaviour, IDamagable, IHammerInteractable, IDestroyable, IRayCastHpDusplayer
    {
        [SerializeField] private NetworkSoundPlayer _soundPlayer;
        [SerializeField] private List<Block> _levels;
        [SerializeField] private float _canbeDestroyedByHammerTime = 60f;

        public Action<IDestroyable> OnDestroyed { get; set; }

        private ConnectedStructure _currentStructure;

        public ConnectedStructure CurrentStructure
        {
            set => _currentStructure = value;
        }

        private NetworkVariable<int> _hp = new(100, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public Block CurrentBlock => _levels[_currentLevel.Value];
        [field: SerializeField] public StructureConnector BuildingConnector { get; private set; }

        [Tooltip("In Seconds")] [SerializeField]
        private float _destroyingTime = 0.1f;

        private NetworkVariable<bool> _canBeDestroyedByHammer = new(true);

        public int StartHp => _startHp;
        private int _startHp;

        private NetworkVariable<ushort> _currentLevel = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private List<InventoryCell> _cellsForRepairing = new List<InventoryCell>();

        private GameObject _activeBlock;


        public override void OnNetworkSpawn()
        {
            InitSlot(_currentLevel.Value);
            _currentLevel.OnValueChanged += (ushort prevValue, ushort newValue) => { InitSlot(newValue); };
            if (IsServer)
                StartCoroutine(HandleDestroyingByHammerTime());
        }

        public override void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
            base.OnDestroy();
        }

        public void PlaySound()
        {
            if (IsServer)
                _soundPlayer.PlayOneShot(CurrentBlock.UpgradingSound);
        }

        private IEnumerator HandleDestroyingByHammerTime()
        {
            _canBeDestroyedByHammer.Value = true;
            yield return new WaitForSeconds(_canbeDestroyedByHammerTime);
            _canBeDestroyedByHammer.Value = false;
        }

        private void InitSlot(int slotId)
        {
            if (_activeBlock != null)
                _activeBlock.SetActive(false);
            var activatingBlock = _levels[_currentLevel.Value];

            PlaySound();

            activatingBlock.gameObject.SetActive(true);
            _activeBlock = activatingBlock.gameObject;
            var gettingHp = (ushort)activatingBlock.GetComponent<Block>().Hp;
            SetHpServerRpc(gettingHp);
            _startHp = gettingHp;
        }

        public List<InventoryCell> GetNeededCellsForPlacing()
            => _levels[_currentLevel.Value].CellForPlace;

        [ServerRpc(RequireOwnership = false)]
        private void SetLevelServerRpc(ushort value)
        {
            _currentLevel.Value = value;
        }


        [ServerRpc(RequireOwnership = false)]
        private void SetHpServerRpc(int value)
        {
            _hp.Value = value;
            if (_hp.Value <= 0)
            {
                if (IsServer)
                    Destroy();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyServerRpc()
        {
            if (!IsServer) return;
            StartCoroutine(DestroyRoutine());
        }

        private IEnumerator DestroyRoutine()
        {
            //Потрібно щоб OnTriggerExit зчитався
            transform.position = new Vector3(1000000, 1000000, 1000000);
            yield return new WaitForSeconds(_destroyingTime);
            if (gameObject == null) yield break;
            var networkObj = GetComponent<NetworkObject>();
            if (networkObj != null)
                networkObj.Despawn();
            Destroy(gameObject);
            if (_currentStructure != null)
                _currentStructure.RemoveBlock(this);
        }

        private bool MaxHp()
            => _hp.Value >= _startHp;

        public void RestoreHealth(int value)
        {
            int hp = _hp.Value + value;
            if (hp > _startHp)
                hp = _startHp;
            SetHpServerRpc((ushort)hp);
        }


        #region IHammerInteractable
        
        public bool CanBeRepaired()
        {
            if (MaxHp()) return false;
            int damagingPercent = 100 - (_hp.Value * 100 / _startHp);
            _cellsForRepairing.Clear();
            foreach (var cell in GetNeededCellsForPlacing())
                _cellsForRepairing.Add(new InventoryCell(cell.Item, cell.Count / damagingPercent));
            return InventoryHandler.singleton.CharacterInventory.EnoughMaterials(_cellsForRepairing);
        }

        public void Repair()
        {
            SetHpServerRpc(_startHp);
            InventoryHandler.singleton.CharacterInventory.RemoveItems(_cellsForRepairing);
            _cellsForRepairing.Clear();
        }

        public bool CanBeDestroyed()
            => _canBeDestroyedByHammer.Value;

        public void Destroy()
            => DestroyServerRpc();

        public void Shake()
        {
        }

        public InventoryCell GetNeededItemsForUpgrade()
        {
            throw new NotImplementedException();
        }

        public bool CanBeUpgraded(int targetLvl)
            => MaxHp() && targetLvl > _currentLevel.Value;

        public List<InventoryCell> GetNeededCellsForUpgrade(int level)
        {
            List<InventoryCell> res = new List<InventoryCell>();
            _levels[level].CellForPlace.ForEach(cell => res.Add(cell));
            return res;
        }

        public void UpgradeTo(int level)
        {
            var cells = _levels[level].CellForPlace;
            InventoryHandler.singleton.CharacterInventory.RemoveItems(cells);
            
            foreach(var slot in cells)
                AlertEventsContainer.OnInventoryItemRemoved?.Invoke(slot.Item.Name, slot.Count);
            SetLevelServerRpc((ushort)level);
        }

        public bool CanBePickUp()
            => false;

        public void PickUp()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IDamagable

        [ServerRpc(RequireOwnership = false)]
        public void GetDamageServerRpc(int damage)
        {
            if(!IsServer) return;
            GetDamage(damage);
            _soundPlayer.PlayOneShot(CurrentBlock.DamageSound, 0.5f);
        }

        public void Decay(int damage)
        {
            GetDamage(damage);  
        }

        public void GetDamage(int damage)
        {
            int hp = _hp.Value - damage;
            _hp.Value = hp;
            if (_hp.Value <= 0)
            {
                if (IsServer)
                    StartCoroutine(DestroyRoutine());
            }
        }

        public AudioClip GetPlayerDamageClip()
            => GlobalSoundsContainer.Singleton.HitSound;

        public int GetHp()
            => _hp.Value;

        public int GetMaxHp()
            => CurrentBlock.Hp;

        #endregion

        public void DisplayData()
            => PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayObjectHp(this);
    }
}