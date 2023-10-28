using UnityEngine;

public class SnapPlacingObjectBP : PlacingObjectBluePrint
{
    public override bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords, out Vector3 rotation)
    {
        Vector3 rayOrigin = targetCamera.transform.position;
        Vector3 rayDirection = targetCamera.transform.forward;
        RaycastHit hit;

        coords = default;
        rotation = default;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _targetMask))
        {
            if (!_placingTags.Contains(hit.collider.tag)) return false;
            coords = hit.collider.transform.position;
            rotation = hit.normal;
            return true;
        }

        return false;
    }
}