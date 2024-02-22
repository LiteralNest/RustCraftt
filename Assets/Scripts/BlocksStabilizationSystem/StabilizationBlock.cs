using System.Collections.Generic;
using UnityEngine;

namespace BlocksStabilizationSystem
{
    public class StabilizationBlock : MonoBehaviour
    {
        private List<StabilizationBlock> _stabilizationBlocks = new List<StabilizationBlock>();
        private bool _isGrounded;

        private void OnDestroy()
        {
            _isGrounded = false;
            CheckStabilization();
        }

        public void SetIsGrounded(bool value)
            => _isGrounded = value;

        private void CheckStabilization()
        {
            if (IsGrounded(this)) return;
            for (int i = 0; i < _stabilizationBlocks.Count; i++)
            {
                var block = _stabilizationBlocks[i];
                if (block == this) return;
                if (block == null || block.gameObject == null) continue;
                _stabilizationBlocks.Remove(block);
                i--;
                Destroy(block.gameObject);
            }
        }

        private bool IsGrounded(StabilizationBlock maskedBlock)
        {
            if (_isGrounded) return true;
            foreach (var block in _stabilizationBlocks)
                if (block != null && block != maskedBlock && block.IsGrounded(this))
                    return true;
            return false;
        }

        public void AssignSnappedBlock(StabilizationBlock stabilizationBlock)
        {
            if (_stabilizationBlocks.Contains(stabilizationBlock) || stabilizationBlock == this) return;
            _stabilizationBlocks.Add(stabilizationBlock);
        }
    }
}