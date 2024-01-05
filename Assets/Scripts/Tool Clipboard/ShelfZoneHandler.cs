using Building_System;
using Building_System.Blocks;
using UnityEngine;

namespace Tool_Clipboard
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class ShelfZoneHandler : MonoBehaviour
    {
        [SerializeField] private ToolClipboard _toolClipboard;
        public ToolClipboard ToolClipboard => _toolClipboard;
        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<Rigidbody>().useGravity = false;
        }

        private void TryAddStructure(BuildingBlock structure)
        {
            if (_toolClipboard.ConnectedBlocks.Contains(structure)) return;
            _toolClipboard.ConnectedBlocks.Add(structure);
        }


        private void CheckBuildingBlockEnter(Collider other)
        {
            var structure = other.GetComponent<BuildingBlock>();
            if (structure == null) return;
            TryAddStructure(structure);
        }

        private void CheckHammerInteractableEnter(Collider other)
        {
            if(other.transform.TryGetComponent(out HammerInteractable hammerInteractable))
                hammerInteractable.TargetToolClipboard = _toolClipboard;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            CheckBuildingBlockEnter(other);
            
        }

        private void OnTriggerExit(Collider other)
        {
            var structure = other.GetComponent<BuildingBlock>();
            if (!_toolClipboard.ConnectedBlocks.Contains(structure)) return;
            _toolClipboard.ConnectedBlocks.Remove(structure);
        }
    }
}
