using UnityEngine;

public class Roof : BuildingBluePrint
{
    [SerializeField] private LayerMask _snapLayer;
    
    private Snap _targetSnap;
    
    private void Start()
    {
        
        Init();
    }
    
    public override void CheckForAvailable()
    {
        CanBePlaced = true;
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
            if (hit.transform.CompareTag("RoofSnap"))
            {
                _targetSnap = hit.transform.GetComponent<Snap>();
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
