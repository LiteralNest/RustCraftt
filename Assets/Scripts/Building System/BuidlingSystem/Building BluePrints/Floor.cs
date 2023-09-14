using System.Collections.Generic;
using UnityEngine;

public class Floor : BuildingBluePrint
{
    [SerializeField] private LayerMask _targetMask;
    private List<GameObject> _triggeredObjects = new List<GameObject>();

    private void Start()
    {
        Init();
    }

    public override void CheckForAvailable()
    {
        if (_triggeredObjects.Count != 0)
        {
            CanBePlaced = false;
            DisplayRenderers();
            return;
        }

        CanBePlaced = InventorySlotsContainer.singleton.ItemsAvaliable(_neededCellsForPlace);
        ;
        DisplayRenderers();
    }

    public override void TriggerEntered(Collider other)
    {
        if (other.gameObject.tag == "Ground") return;
        _triggeredObjects.Add(other.gameObject);
        CheckForAvailable();
    }

    public override void TriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground") return;
        _triggeredObjects.Remove(other.gameObject);
        CheckForAvailable();
    }

    public override bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords)
    {
        Vector3 rayOrigin = targetCamera.transform.position;
        Vector3 rayDirection = targetCamera.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _targetMask))
        {
            if (hit.transform.CompareTag("Block"))
            {
                var structureSize = _targetBuildingStruncture.StructureSize;
                int x = Mathf.RoundToInt(hit.point.x + hit.normal.x / (2 / structureSize.x));
                int y = Mathf.RoundToInt(hit.point.y + hit.normal.y / (2 / structureSize.y));
                int z = Mathf.RoundToInt(hit.point.z + hit.normal.z / (2 / structureSize.z));
                coords = new Vector3(x, y, z);
                return true;
            }
        }

        coords = default;
        return false;
    }
}