using Unity.Netcode;
using UnityEngine;

namespace StabilizationSystem.Blocks
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class BlockSnap : NetworkBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private StabilizationBlock _stabilizationBlock;

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
            if(!IsServer) return;
            CheckBlockSnapEnter(other);
            CheckGroundEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!IsServer) return;
            CheckGroundExit(other);
        }

        private void CheckBlockSnapEnter(Collider other)
        {
            if (!other.TryGetComponent(out BlockSnap blockSnap)) return;
            _stabilizationBlock.AssignSnappedBlock(blockSnap._stabilizationBlock);
        }

        private void CheckGroundEnter(Collider other)
        {
            if (!other.CompareTag("Ground")) return;
            _stabilizationBlock.SetIsGrounded(true);
        }

        private void CheckGroundExit(Collider other)
        {
            if (!other.CompareTag("Ground")) return;
            _stabilizationBlock.SetIsGrounded(false);
        }
    }
}