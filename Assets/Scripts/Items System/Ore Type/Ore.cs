using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Items_System.Ore_Type
{
    public class Ore : NetworkBehaviour
    {
        [Header("Start init")]
        [SerializeField] protected int _hp = 100;
        [SerializeField] protected NetworkVariable<int> _currentHp = new(100, NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
    
        [SerializeField] private float _recoveringSpeed;
        [SerializeField] protected List<OreSlot> _resourceSlots = new List<OreSlot>();
        [SerializeField] private List<GameObject> _renderers;

        public bool Recovering { get; protected set; } = false;

        public override void OnNetworkSpawn()
        {
            _currentHp.Value = _hp;
            _currentHp.OnValueChanged += (int prevValue, int newValue) =>
            {
                CheckHp();
            };
            base.OnNetworkSpawn();
        }
    
        protected void AddResourcesToInventory()
        {
            foreach(var slot in _resourceSlots)
                InventoryHandler.singleton.CharacterInventory.AddItemToDesiredSlotServerRpc(slot.Resource.Id, Random.Range(slot.CountRange.x, slot.CountRange.y + 1), 0);
        }
    
        private void CheckHp()
        {
            if (_currentHp.Value > 0) return;
            DestroyObject();
        }
    
        protected void TurnRenderers(bool value)
        {
            foreach (var renderer in _renderers)
                renderer.SetActive(value);
        }
    
        private IEnumerator Recover()
        {
            Recovering = true;
            yield return new WaitForSeconds(_recoveringSpeed);
            Recovering = false;
            TurnRenderers(true);
        }

        protected virtual void DestroyObject()
            => StartCoroutine( Destroy());
    
        protected IEnumerator Destroy()
        {
            TurnRenderers(false);
            yield return Recover();
            if(NetworkManager.Singleton.IsServer)
                _currentHp.Value = _hp;
        }
    
        [ServerRpc(RequireOwnership = false)]
        public void MinusHpServerRpc()
        {
            if (_currentHp.Value <= 0) return;
            _currentHp.Value--;
        }
    }
}
