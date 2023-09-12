using UnityEngine;
using UnityEngine.Serialization;

public class ObjectBluePrint : BluePrint
{
    [Header("Start Init")] 
    [SerializeField] private float _distance = 2f;
    [SerializeField] private LayerMask _snapLayerMask;
    [SerializeField] private string _requiredTag;
    
    public override void CheckForAvailable()
    {
        
    }

    private Vector3 GetFrontOfCameraPosition(Camera targetCamera)
        => targetCamera.transform.position + targetCamera.transform.forward * _distance;
    
    public override bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords)
    {
        coords = default;
        Vector3 rayOrigin = targetCamera.transform.position;
        Vector3 rayDirection = targetCamera.transform.forward;
        RaycastHit hit;

        var targetTransform = transform;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _snapLayerMask))
        {
            if (hit.transform.CompareTag(_requiredTag))
            {
                if (!hit.transform.TryGetComponent<Snap>(out var snap)) return false;
                targetTransform.position = snap.transform.position;
                targetTransform.rotation = snap.transform.rotation;
                CanBePlaced = true;
                return true;
            }
        }

        CanBePlaced = false;
        targetTransform.position = GetFrontOfCameraPosition(targetCamera);
        return false;
    }

    public bool TryPlaceObject()
    {
        if(!TryPlace()) return false;
        Destroy(gameObject);
        return true;
    }
}