using System;
using System.Collections;
using System.Collections.Generic;
using AlertsSystem;
using Building_System.Upgrading;
using FightSystem.Damage;
using InteractSystem;
using Inventory_System;
using Player_Controller;
using Sound_System;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Building.Blocks
{
    public class BuildingBlock : NetworkBehaviour, IBuildingDamagable, IHammerInteractable, IDestroyable,
        IRayCastHpDisplayer
    {
        [SerializeField] private NetworkSoundPlayer _soundPlayer;
        [SerializeField] private List<Block> _levels;
        [SerializeField] private float _canbeDestroyedByHammerTime = 60f;

        public Action<IDestroyable> OnDestroyed { get; set; }

        private NetworkVariable<float> _hp = new(100, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public Block CurrentBlock => _levels[_currentLevel.Value];

        [Tooltip("In Seconds")] [SerializeField]
        private float _destroyingTime = 0.1f;

        private NetworkVariable<bool> _canBeDestroyedByHammer = new(true);
        private int _startHp;

        private NetworkVariable<ushort> _currentLevel = new(0, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private List<InventoryCell> _cellsForRepairing = new List<InventoryCell>();

        private GameObject _activeBlock;
        private Coroutine _decayCoroutine;

        public override void OnNetworkSpawn()
        {
            InitSlot();
            _currentLevel.OnValueChanged += (ushort prevValue, ushort newValue) => { InitSlot(); };
            if (IsServer)
                StartCoroutine(HandleDestroyingByHammerTime());
        }

        public override void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
            base.OnDestroy();
        }

        private void Start()
        {
            if (IsServer)
                _decayCoroutine = StartCoroutine(DecayRoutine());
        }

        public void ToolClipBoardAssign(bool value)
        {
            if (value)
            {
                if (_decayCoroutine != null)
                    StopCoroutine(_decayCoroutine);
            }
            else
                _decayCoroutine = StartCoroutine(DecayRoutine());
        }

        private IEnumerator DecayRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(60);
                Decay();
            }
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

        private void InitSlot()
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

        public void Destroy()
        {
            if (IsServer)
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
            _soundPlayer.PlayOneShot(CurrentBlock.DestroyingSound);
            Destroy(gameObject);
        }

        private bool MaxHp()
            => _hp.Value >= _startHp;


        #region IHammerInteractable

        public bool CanBeRepaired()
        {
            if (MaxHp()) return false;
            var damagingPercent = 100 - (_hp.Value * 100 / _startHp);
            _cellsForRepairing.Clear();
            foreach (var cell in GetNeededCellsForPlacing())
            {
                int count = 1;
                if((int)damagingPercent != 0)
                    count = cell.Count / (int)damagingPercent;
                if (count <= 0) count = 1;
                _cellsForRepairing.Add(new InventoryCell(cell.Item, count));
            }

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

            foreach (var slot in cells)
                AlertEventsContainer.OnInventoryItemRemoved?.Invoke(slot.Item.Name, slot.Count);
            SetLevelServerRpc((ushort)level);
        }

        public bool CanBePickUp()
            => false;

        public void PickUp()
        {
        }

        #endregion

        #region IBuildingDamagable

        private void AssignDamage(float damage)
        {
            float hp = _hp.Value - damage;
            _hp.Value = hp;
            if (_hp.Value <= 0)
            {
                if (IsServer)
                    StartCoroutine(DestroyRoutine());
            }
        }

        public void Decay()
        {
            var damage = CurrentBlock.StartHp / CurrentBlock.DecayHoursTime / 60f;
            AssignDamage(damage);
        }

        public void GetDamageOnServer(int itemId)
        {
            if (!IsServer) return;
            var damage = CurrentBlock.GetDamageAmount(itemId);
            AssignDamage(damage);
            if (_hp.Value - damage > 0)
                _soundPlayer.PlayOneShot(CurrentBlock.DamageSound);
        }

        public void GetDamageByExplosive(int explosiveId, float distance, float radius)
        {
            var damage = CurrentBlock.GetDamageAmountByExplosive(explosiveId, distance, radius);
            AssignDamage(damage);
        }

        public int GetHp()
            => Mathf.RoundToInt(_hp.Value);

        public int GetMaxHp()
            => CurrentBlock.Hp;

        #endregion

        public void DisplayData()
        {
            if(PlayerNetCode.Singleton == null) return;
             PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayBuildingHp(this);
        }
    }
}