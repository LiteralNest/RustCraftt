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

      private Vector3 _cachedPosition;
      private Quaternion _cachedRotation;

      private void ChangeBodyPosition()
      {
         _corpesObject.transform.SetPositionAndRotation(_corpesPlace.position, _corpesPlace.rotation);
         _cachedPosition = _corpesPlace.position;
         _cachedRotation = _corpesPlace.rotation;
      }
      
      public void ReturnToDefaultPosition()
      {
         _corpesObject.transform.SetPositionAndRotation(_cachedPosition, _cachedRotation);
         _bodyAnimationsHandler.SetIdle();
      }

      public void AssignKnockDown()
      {
         ChangeBodyPosition();
         _bodyAnimationsHandler.SetKnockDown();
      }
      
      public void AssignCorpes(CustomSendingInventoryData data)
      {
         ChangeBodyPosition();
         _bodyAnimationsHandler.SetDeath();
         if(!IsServer) return;
         _corpesObject.AssignCells(data);
      }
   }
}
