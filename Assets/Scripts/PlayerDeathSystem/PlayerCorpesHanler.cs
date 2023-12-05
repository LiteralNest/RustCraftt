using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
   public class PlayerCorpesHanler : NetworkBehaviour
   {
      [SerializeField] private BackPack _corpesObject;
      [SerializeField] private Transform _corpesPlace;

      public void AssignCorpes(CustomSendingInventoryData data)
      {
         _corpesObject.transform.SetPositionAndRotation(_corpesPlace.position, _corpesPlace.rotation);
         if(!IsServer) return;
         _corpesObject.AssignCells(data);
      }
   }
}
