using Building_System.Building.Blocks;
using Cloud.DataBaseSystem.UserData;
using Inventory_System;
using Lock_System;
using UnityEngine;

namespace Building_System.Building.Placing_Objects
{
    public class KeySnapPlacingObjectBp : SnapPlacingObjectBp
    {
        public override void InitPlacedObject(BuildingStructure structure)
        {
            var locker = structure.GetComponent<Locker>();
            if (!locker)
            {
                Debug.LogError("Can't load KeyLocker!");
                return;
            }
            locker.Init(UserDataHandler.Singleton.UserData.Id);
        }

        public override void Place()
        {
            if (!CanBePlaced()) return;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(TargetPlacingObject.TargetItem.Id, 1);
            var instance = Instantiate(TargetPlacingObject, transform.position, transform.rotation);
        }
    }
}