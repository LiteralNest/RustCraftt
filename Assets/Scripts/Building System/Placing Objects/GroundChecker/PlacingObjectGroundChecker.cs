using UnityEngine;

namespace Building_System.Placing_Objects.GroundChecker
{
    public class PlacingObjectGroundChecker : MonoBehaviour
    {
        private LayerMask _rayCastLayer;
        private PlacingObjectGroundHandler _handler;

        public void Init(LayerMask rayCastLayer, PlacingObjectGroundHandler handler)
        {
            _rayCastLayer = rayCastLayer;
            _handler = handler;
            RayCastTarget();
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2);
        }

#endif

        private void RayCastTarget()
        {
            var ray = new Ray(transform.position, Vector3.down);
            if (!Physics.Raycast(ray, out var hit, 2, _rayCastLayer)) return;
            var target = hit.transform.GetComponent<IDestroyable>();
            if (target == null) return;
            _handler.AddTarget(target);
        }
    }
}