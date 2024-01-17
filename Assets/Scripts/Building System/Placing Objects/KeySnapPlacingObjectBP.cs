using Building_System.Blocks;
using Lock_System;
using UnityEngine;
using Web.UserData;

namespace Building_System.Placing_Objects
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