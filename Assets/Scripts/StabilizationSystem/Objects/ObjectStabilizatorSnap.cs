using StabilizationSystem.Blocks;
using UnityEngine;

namespace StabilizationSystem.Objects
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class ObjectStabilizatorSnap : MonoBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private ObjectStabilizator _objectStabilizator;

        private StabilizationBlock _stabilizationBlock;
        public StabilizationBlock StabilizationBlock => _stabilizationBlock;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var collider = GetComponent<BoxCollider>();
            var size = collider.size;
            Gizmos.DrawCube(transform.position, new Vector3(size.x, size.y, size.z));
        }
#endif

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out StabilizationBlock stabilizationBlock)) return;
            stabilizationBlock.OnBlockDestroyed += _objectStabilizator.TryDestroy;
            _stabilizationBlock = stabilizationBlock;
        }
    }
}