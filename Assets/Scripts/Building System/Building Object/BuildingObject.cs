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
   }

   [ContextMenu("Upgrade")]
   private void TryUpgrade()
   {
      if(_currentLevel == _slots.Count - 1) return;
      _currentLevel++;
      InitSlot(_currentLevel);
   }
}
