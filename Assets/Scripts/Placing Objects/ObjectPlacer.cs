using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private LayerMask _snapLayer;

    [field:SerializeField] public PlacingObject Target { get; set; }
    private Camera _targetCamera;

    private void Start()
        => _targetCamera = Camera.main;

    private void FixedUpdate()
    {
        if(Target == null) return;
        TryRayCast();
    }
    
    private void TryRayCast()
    {
        Vector3 rayOrigin = _targetCamera.transform.position;
        Vector3 rayDirection = _targetCamera.transform.forward;
        RaycastHit hit;
    
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _snapLayer))
        {
            if (hit.transform.CompareTag(Target.RequiredTag))
            {
                if (!hit.transform.TryGetComponent<Snap>(out var snap)) return;
                var targetTransform = Target.transform;
                targetTransform.position = snap.transform.position;
                targetTransform.rotation = snap.transform.rotation;
            }
        }
    }
}
