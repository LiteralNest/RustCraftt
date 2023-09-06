using UnityEngine;

public class Wall : BuildingBluePrint
{
    [SerializeField] private LayerMask _snapLayer;

    private SnapPoint _targetSnap;

    private void Start()
    {
        Init();
    }
    
    public override void CheckForAvailable()
    {
        if (_targetSnap == null)
        {
            CanBePlaced = false;
            DisplayRenderers();
            return;
        }
        CanBePlaced = InventorySlotsContainer.singleton.ItemsAvaliable(_neededCellsForPlace);
        DisplayRenderers();
    }

    public override bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords)
    {
        _targetSnap = null;
        CheckForAvailable();
        
        Vector3 rayOrigin = targetCamera.transform.position;
        Vector3 rayDirection = targetCamera.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _snapLayer))
        {
            if (hit.transform.CompareTag("Snap"))
            {
                _targetSnap = hit.transform.GetComponent<SnapPoint>();
                coords = hit.transform.position;
                transform.eulerAngles = _targetSnap.TargetRotation;
                CheckForAvailable();
                return true;
            }
        }
        coords = default;
        return false;
    }
}
