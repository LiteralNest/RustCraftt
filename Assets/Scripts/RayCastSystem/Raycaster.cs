using UnityEngine;

namespace RayCastSystem
{
    public class Raycaster
    {
        private GameObject GetRayCastedObject<T>(LayerMask layer, float hitDistance, out RaycastHit hit) where T : MonoBehaviour
        {
            hit = default;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, hitDistance, layer))
            {
                hit = hitInfo;
                return hitInfo.collider.gameObject;
            }
            return null;
        }

        public bool TryRaycast<T>(string tag, float hitDistance, out T target, LayerMask layer, out RaycastHit hitInfo) where T : MonoBehaviour
        {
            target = default;
            var rayCastedObject = GetRayCastedObject<T>(layer, hitDistance, out hitInfo);
            if (rayCastedObject == null) return false;
            target = rayCastedObject.GetComponent<T>();
            if(target == null) return false;
            if (!rayCastedObject.CompareTag(tag)) return false;
            return true;
        }
    }
}