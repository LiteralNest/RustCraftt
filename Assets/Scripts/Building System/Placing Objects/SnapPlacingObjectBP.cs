using Building_System.Blocks;
using UnityEngine;

namespace Building_System.Placing_Objects
{
    public class SnapPlacingObjectBp : PlacingObjectBluePrint
    {
        public override bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords, out Quaternion rotation, out bool shouldRotate, float distance)
        {
            shouldRotate = true;
        
            Vector3 rayOrigin = targetCamera.transform.position;
            Vector3 rayDirection = targetCamera.transform.forward;
            RaycastHit hit;
       
            coords = default;
            rotation = default;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _targetMask))
            {
                if (!_placingTags.Contains(hit.collider.tag)) return false;
                coords = hit.collider.transform.position;
                rotation = hit.collider.transform.rotation;
                return true;
            }

            return false;
        }

        public override void InitPlacedObject(BuildingStructure structure)
        {
        }
    }
}