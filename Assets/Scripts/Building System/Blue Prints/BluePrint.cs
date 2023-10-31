using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BluePrint : MonoBehaviour
{
    [Header("Main Params")]
    [SerializeField] protected Vector3Int _buildingSize = new Vector3Int(1, 1, 1);
    [Header("Renderers")]
    public List<BuildingBluePrintCell> BluePrintCells = new List<BuildingBluePrintCell>();
    [Header("Layers")] 
    [SerializeField] protected LayerMask _targetMask;
    [SerializeField] protected List<string> _placingTags = new List<string>();

    protected bool _rotatedSide;
    
    #region Abstract

    public abstract void Place();

    #endregion


    public virtual bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords, out Vector3 rotation)
    {
        Vector3 rayOrigin = targetCamera.transform.position;
        Vector3 rayDirection = targetCamera.transform.forward;
        RaycastHit hit;
        rotation = default;
        coords = default;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _targetMask))
        {
            if (!_placingTags.Contains(hit.collider.tag)) return false;
            int x, y, z;
            var structureSize = _buildingSize;

            y = Mathf.RoundToInt(hit.point.y + hit.normal.y / (2 / structureSize.y));

            if (_rotatedSide)
            {
                x = Mathf.RoundToInt(hit.point.x + hit.normal.x / (2 / structureSize.z));
                z = Mathf.RoundToInt(hit.point.z + hit.normal.z / (2 / structureSize.x));
            }
            else
            {
                x = Mathf.RoundToInt(hit.point.x + hit.normal.x / (2 / structureSize.x));
                z = Mathf.RoundToInt(hit.point.z + hit.normal.z / (2 / structureSize.z));
            }

            coords = new Vector3(x, y, z);
            return true;
        }

        coords = default;
        return false;
    }

    public void Rotate()
    {
        transform.eulerAngles += new Vector3(0, 90, 0);
        _rotatedSide = !_rotatedSide;
    }

    public void SetOnFrontOfPlayer(bool value)
    {
        foreach (var cell in BluePrintCells)
        {
            cell.OnFrontOfPlayer = value;
            cell.CheckForAvailable();
        }
    }
}