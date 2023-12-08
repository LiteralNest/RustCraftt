using Animation_System;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
   public class PlayerCorpesHanler : NetworkBehaviour
   {
      [SerializeField] private CharacterAnimationsHandler _bodyAnimationsHandler;
      [SerializeField] private BackPack _corpesObject;
      [SerializeField] private Transform _corpesPlace;
      

      private void ChangeBodyPosition()
      {
         _corpesObject.transform.SetPositionAndRotation(_corpesPlace.position, _corpesPlace.rotation); 
      }
      
      
      public void AssignCorpes(CustomSendingInventoryData data)
      {
         ChangeBodyPosition();
         // _bodyAnimationsHandler.SetDeath();
         if(!IsServer) return;
         _corpesObject.AssignCells(data);
      }
   }
}
