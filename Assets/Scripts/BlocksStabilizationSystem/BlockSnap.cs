using System;
using UnityEngine;

namespace BlocksStabilizationSystem
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class BlockSnap : MonoBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private StabilizationBlock _stabilizationBlock;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var collider = GetComponent<BoxCollider>();
            Gizmos.DrawCube(transform.position, new Vector3(collider.size.x, collider.size.y, collider.size.z));
        }
#endif

        private void OnTriggerEnter(Collider other)
        {
            CheckBlockSnapEnter(other);
            CheckGroundEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
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