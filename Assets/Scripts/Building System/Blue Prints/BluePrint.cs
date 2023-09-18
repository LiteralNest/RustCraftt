using System.Collections.Generic;
using UnityEngine;

public abstract class BluePrint : MonoBehaviour
{
    public BuildingStructure TargetBuildingStructure;

    [Header("Renderers")] 
    [SerializeField] protected List<BuildingBluePrintCell> _bluePrintCells = new List<BuildingBluePrintCell>();

    [Header("Layers")]
    [SerializeField] protected LayerMask _targetMask;
    public bool CanBePlaced { get; protected set; }
    
        
    protected bool _rotatedSide;
    
    #region Abstract
    public abstract void CheckForAvailable();
    public abstract void Place();
    #endregion
    
    #region Virtual
    public virtual void TriggerEntered(Collider other){}
    public virtual void TriggerExit(Collider other){}
    #endregion

    public bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords)
    {
        Vector3 rayOrigin = targetCamera.transform.position;
        Vector3 rayDirection = targetCamera.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _targetMask))
        {
            if (hit.transform.CompareTag("Block"))
            {
                int x, y, z;
                var structureSize = TargetBuildingStructure.StructureSize;
                
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
        }

        coords = default;
        return false;
    }

    public void Rotate()
    {
        transform.eulerAngles += new Vector3(0, 90, 0);
        _rotatedSide = !_rotatedSide;
    }
    
    public bool TryPlace()
    {
        if (!CanBePlaced)
            return false;
        Place();
        return true;
    }
    
    private void OnTriggerEnter(Collider other)
        => TriggerEntered(other);

    private void OnTriggerExit(Collider other)
        => TriggerExit(other);

    public void SetCanBePlaced(bool value)
        => CanBePlaced = value;
}
