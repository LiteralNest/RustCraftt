using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
   [Header("Slots")]
   [SerializeField] private List<BuildingObjectSlot> _slots = new List<BuildingObjectSlot>();
   
   [Header("Test")]
   [SerializeField] private int _currentHp;

   private int _currentLevel = 0;
   private GameObject _currentObj;

   private void Start()
   {
      InitSlot(0);
   }

   private void InitSlot(int id)
   {
      if(_currentObj != null)
         _currentObj.SetActive(false);
      _currentObj = _slots[id].TargetObject;
      _currentObj.SetActive(true);
      _currentHp = _slots[id].Hp;
      foreach (var cell in _slots[_currentLevel].NeededCellsForPlace)
         InventorySlotsContainer.singleton.DeleteSlot(cell.Item, cell.Count);
   }

   public bool CanBeUpgrade()
   {
      if (_currentLevel + 1 >= _slots.Count) return false;
      if(!InventorySlotsContainer.singleton.ItemsAvaliable(_slots[_currentLevel + 1].NeededCellsForPlace)) return false;
      return true;
   }

   public void TryUpgrade()
   {
      if(!CanBeUpgrade()) return;
      _currentLevel++;
      InitSlot(_currentLevel);
   }

   public void ReturnMaterialsToInventory()
   {
      foreach (var cell in _slots[0].NeededCellsForPlace)
         InventorySlotsContainer.singleton.AddItemToDesiredSlot(cell.Item, (int)(cell.Count / 2));
   }
}
