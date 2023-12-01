using Storage_System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerDeathSystem
{
   public class PlayerBackPackGenerator : NetworkBehaviour
   {
      [FormerlySerializedAs("_backPackPref")] [SerializeField] private BackPack playerBackPackPref;
   
      [ServerRpc(RequireOwnership = false)]
      public void GenerateBackPackServerRpc(Vector3 position, CustomSendingInventoryData playerInvData)
      {
         if(!IsServer) return;

         var backPack = Instantiate(playerBackPackPref, position, Quaternion.identity);
         backPack.GetComponent<NetworkObject>().Spawn();
         backPack.AssignCells(playerInvData);
         var playerInv = InventoryHandler.singleton.CharacterInventory;
      }
   }
}
