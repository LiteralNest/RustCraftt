using Building_System.Building.Blocks;
using UnityEngine;

namespace Building_System.Blue_Prints
{
    public class BuildingBluePrint : BluePrint
    {
        [field: SerializeField] public Vector3Int StructureOffset { get; private set; } = Vector3Int.zero;

        public override void Place()
        {
            if (!EnoughMaterials()) return;

            bool playedSound = false;

            foreach (var cell in BluePrintCells)
            {
                cell.TryPlace(!playedSound);
                playedSound = true;
            }
        }

        public override void InitPlacedObject(BuildingStructure structure)
        {
        }

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

                int rotx = StructureOffset.x * (int)hit.normal.x;
                int roty = StructureOffset.y * (int)hit.normal.y;
                int rotz = StructureOffset.z * (int)hit.normal.z;

                coords = new Vector3(x + rotx, y + roty, z + rotz);
                return true;
            }


            coords = default;
            return false;
        }
    }
}