using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerDeathSystem
{
   public class PlayerBackPackGenerator : NetworkBehaviour
   {
      [FormerlySerializedAs("_backPackPref")] [SerializeField] private PlayerBackPack playerBackPackPref;
   
      [ServerRpc(RequireOwnership = false)]
      public void GenerateBackPackServerRpc(Vector3 position)
      {
         if(!IsServer) return;

         var backPack = Instantiate(playerBackPackPref, position, Quaternion.identity);
         backPack.GetComponent<NetworkObject>().Spawn();
         var playerInv = InventoryHandler.singleton.CharacterInventory;
         backPack.AssignCells(playerInv.ItemsNetData.Value);
         playerInv.ResetItemsServerRpc();
      }
   }
}
