using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerDeathSystem
{
   public class PlayerBackPackGenerator : MonoBehaviour
   {
      [FormerlySerializedAs("_backPackPref")] [SerializeField] private PlayerBackPack playerBackPackPref;
   
      public void GenerateBackPack(Transform place)
      {
         var backPack = Instantiate(playerBackPackPref, place.position, place.rotation); //Переписати під мультиплеєр
         var playerInv = InventoryHandler.singleton.CharacterInventory;
         backPack.AssignCells(playerInv.ItemsNetData);
         playerInv.ResetItemsServerRpc();
      }
   }
}
