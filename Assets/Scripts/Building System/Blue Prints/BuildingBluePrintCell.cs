using System.Collections.Generic;
using AlertsSystem;
using Building_System.Blocks;
using Building_System.NetWorking;
using UnityEngine;

namespace Building_System.Blue_Prints
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class BuildingBluePrintCell : MonoBehaviour
    {
        [Header("Start Init")] [SerializeField]
        private BluePrint _bluePrint;

        [SerializeField] private BuildingStructure _targetBuildingStructure;
        public BuildingStructure TargetBuildingStructure => _targetBuildingStructure;
        [Header("Materials")] [SerializeField] private Material _negativeMaterial;
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private List<Renderer> _renderers;

        private List<GameObject> _triggeredObjects = new List<GameObject>();
        public bool CanBePlaced { get; private set; }

        public bool OnFrontOfPlayer { get; set; }

        public bool EnoughMaterials { get; set; }
        
        private void SetMaterial(Material material)
        {
            foreach (var renderer in _renderers)
                renderer.sharedMaterial = material;
        }

        public virtual bool CanBePlace()
            => !(!EnoughMaterials || _triggeredObjects.Count > 0 || OnFrontOfPlayer);

        private void SetCanBePlaced(bool value)
        {
            CanBePlaced = value;
            if (!CanBePlaced)
                SetMaterial(_negativeMaterial);
            else
                SetMaterial(_normalMaterial);
        }

        public void CheckForAvailable()
        {
            if (!CanBePlace())
            {
                SetCanBePlaced(false);
                return;
            }
            SetCanBePlaced(true);
        }

        public void TryPlace(bool shouldPlaySound)
        {
            if (!CanBePlaced) return;

            foreach (var cell in _targetBuildingStructure.GetPlacingRemovingCells())
            {
                InventoryHandler.singleton.CharacterInventory.RemoveItem((ushort)cell.Item.Id, (ushort)cell.Count);
                AlertEventsContainer.OnInventoryItemRemoved?.Invoke(cell.Item.Name, cell.Count);
            }
            
            BuildingsNetworkingSpawner.singleton.SpawnPrefServerRpc(_targetBuildingStructure.Id, transform.position,
                transform.rotation, shouldPlaySound);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NoBuild"))
            {
                SetCanBePlaced(false);
                return;
            }

            if (other.CompareTag("ConnectingPoint") || other.CompareTag("ShelfZone") || other.CompareTag("WorkBench")) return;
            if (_triggeredObjects.Contains(other.gameObject)) return;
            _triggeredObjects.Add(other.gameObject);
            CheckForAvailable();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ConnectingPoint") || other.CompareTag("ShelfZone") || other.CompareTag("WorkBench")) return;
            if (!_triggeredObjects.Contains(other.gameObject)) return;
            _triggeredObjects.Remove(other.gameObject);
            CheckForAvailable();
        }
    }
}