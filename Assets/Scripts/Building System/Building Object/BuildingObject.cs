using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BuildingObject : NetworkBehaviour
{
   [Header("Slots")]
   [SerializeField] private List<BuildingObjectSlot> _slots = new List<BuildingObjectSlot>();

   [Header("Test")]
   [SerializeField] private int _currentHp;

   private NetworkVariable<int> _currentLevel = new NetworkVariable<int>(0);
   private GameObject _currentObj;

   private void Start()       
   {
      InitSlot(_currentLevel.Value);
      _currentLevel.OnValueChanged += (int prevValue, int newValue) =>
      {
         InitSlot(newValue);
      };
   }

   private void InitSlot(int id)
   {
      if(_currentObj != null)
         _currentObj.SetActive(false);
      _currentObj = _slots[id].TargetObject;
      _currentObj.SetActive(true);
      _currentHp = _slots[id].Hp;
      // foreach (var cell in _slots[_currentLevel].NeededCellsForPlace)
      //    InventorySlotsContainer.singleton.DeleteSlot(cell.Item, cell.Count);
   }

   public bool CanBeUpgrade()
   {
      if (_currentLevel.Value + 1 >= _slots.Count) return false;
      if(!InventorySlotsContainer.singleton.ItemsAvaliable(_slots[_currentLevel.Value + 1].NeededCellsForPlace)) return false;
      return true;
   }

   [ServerRpc(RequireOwnership = false)]
   private void UpgradeServerRpc()
   {
      if(!IsServer) return;
      _currentLevel.Value++;
     
   }
   
   public void TryUpgrade()
   {
      if(!CanBeUpgrade()) return; ;
      UpgradeServerRpc();
   }

   public void ReturnMaterialsToInventory()
   {
      foreach (var cell in _slots[0].NeededCellsForPlace)
         InventorySlotsContainer.singleton.AddItemToDesiredSlot(cell.Item, (int)(cell.Count / 2));
   }
}
