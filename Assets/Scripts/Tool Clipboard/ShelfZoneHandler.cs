using System.Collections;
using System.Collections.Generic;
using Building_System;
using Building_System.Building.Blocks;
using Unity.Netcode;
using UnityEngine;

namespace Tool_Clipboard
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class ShelfZoneHandler : NetworkBehaviour
    {
        [SerializeField] private List<BuildingBlock> _connectedBlocks = new List<BuildingBlock>();
        public List<BuildingBlock> ConnectedBlocks => _connectedBlocks;
        [SerializeField] private ToolClipboard _toolClipboard;
        public ToolClipboard ToolClipboard => _toolClipboard;

        private void Start()
            => StartCoroutine(DecayRoutine());

        private IEnumerator DecayRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(60);
                if (_toolClipboard.GetAvailableHours() <= 0)
                {
                    foreach (var block in _connectedBlocks)
                        block.Decay();
                }
                else
                    _toolClipboard.RemoveItems(_toolClipboard.GetNeededResourcesForHour());
            }
        }

        private void TryAddBlock(BuildingBlock structure)
        {
            if (_connectedBlocks.Contains(structure)) return;
            _connectedBlocks.Add(structure);
            structure.ToolClipBoardAssign(true);
        }

        private void CheckBuildingBlockEnter(Collider other)
        {
            var structure = other.GetComponent<BuildingBlock>();
            if (structure == null) return;
            TryAddBlock(structure);
        }

        private void CheckHammerInteractableEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out HammerInteractable hammerInteractable))
                hammerInteractable.TargetToolClipboard = _toolClipboard;
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckBuildingBlockEnter(other);
            CheckHammerInteractableEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            var structure = other.GetComponent<BuildingBlock>();
            if (!_connectedBlocks.Contains(structure)) return;
            structure.ToolClipBoardAssign(false);
            _connectedBlocks.Remove(structure);
        }
    }
}