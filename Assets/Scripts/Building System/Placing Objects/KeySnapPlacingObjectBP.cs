using Building_System.Blocks;
using Building_System.NetWorking;
using Lock_System;
using UnityEngine;
using Web.User;

namespace Building_System.Placing_Objects
{
    public class KeySnapPlacingObjectBP : SnapPlacingObjectBP
    {
        public override void InitPlacedObject(BuildingStructure structure)
        {
            var locker = structure.GetComponent<KeyLocker>();
            if (!locker)
            {
                Debug.LogError("Can't load KeyLocker!");
                return;
            }

            locker.RegistrateKey(UserDataHandler.singleton.UserData.Id);
        }

        public override void Place()
        {
            if (!CanBePlaced()) return;
            PlacingObjectsPool.singleton.InstantiateObjectServerRpc(TargetPlacingObject.TargetItem.Id,
                transform.position,
                transform.rotation, 
                UserDataHandler.singleton.UserData.Id);
        }
    }
}