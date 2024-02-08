using Building_System.Blue_Prints;
using Building_System.Building.Blocks;
using Building_System.NetWorking;
using Inventory_System;
using UnityEngine;
using Web.UserData;

namespace Building_System.Building.Placing_Objects
{
    public class PlacingObjectBluePrint : BluePrint
    {
        public PlacingObject TargetPlacingObject;
        [SerializeField] protected bool _shouldLoadOwnerId;

        protected virtual bool CanBePlaced()
        {
            foreach (var cell in BluePrintCells)
                if (!cell.CanBePlaced)
                    return false;
            return true;
        }

        public override void Place()
        {
            if (!CanBePlaced()) return;
            var ownerId = -1;
            if(_shouldLoadOwnerId)
                ownerId = UserDataHandler.Singleton.UserData.Id;
            InventoryHandler.singleton.CharacterInventory.RemoveItem(TargetPlacingObject.TargetItem.Id,
                1);
            PlacingObjectsPool.singleton.InstantiateObjectServerRpc(TargetPlacingObject.TargetItem.Id,
                transform.position,
                transform.rotation,
            UserDataHandler.Singleton.UserData.Id);
        }

        public override void InitPlacedObject(BuildingStructure structure){}
        
        public override bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords, out Quaternion rotation,
            out bool shouldRotate, float distance)
        {
            shouldRotate = false;
            Vector3 rayOrigin = targetCamera.transform.position;
            Vector3 rayDirection = targetCamera.transform.forward;
            RaycastHit hit;
            rotation = default;
            coords = default;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, distance, _targetMask))
            {
                if (!_placingTags.Contains(hit.collider.tag)) return false;

                if (hit.normal != Vector3.up) return false;
                
                int x, y, z;
                y = Mathf.RoundToInt(hit.point.y + hit.normal.y / 2);
                
                
                if (_rotatedSide)
                {
                    x = Mathf.RoundToInt(hit.point.x + hit.normal.x / 2);
                    z = Mathf.RoundToInt(hit.point.z + hit.normal.z / 2);
                }
                else
                {
                    x = Mathf.RoundToInt(hit.point.x + hit.normal.x / 2);
                    z = Mathf.RoundToInt(hit.point.z + hit.normal.z / 2);
                }
                
                coords = new Vector3(x, y, z);
                return true;
            }

            coords = default;
            return false;
        }
    }
}