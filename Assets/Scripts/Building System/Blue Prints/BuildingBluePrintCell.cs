using System;
using System.Collections.Generic;
using AlertsSystem;
using Building_System.Building.Blocks;
using Building_System.NetWorking;
using Inventory_System;
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
            => SetCanBePlaced(CanBePlace());

        public void TryPlace(bool shouldPlaySound)
        {
            if (!CanBePlaced) return;

            foreach (var cell in _targetBuildingStructure.GetPlacingRemovingCells())
            {
                InventoryHandler.singleton.CharacterInventory.RemoveItem((ushort)cell.Item.Id, (ushort)cell.Count);
                AlertEventsContainer.OnInventoryItemRemoved?.Invoke(cell.Item.Name, cell.Count);
            }

            BuildingsNetworkingSpawner.Singleton.SpawnPrefServerRpc(_targetBuildingStructure.Id, transform.position,
                transform.rotation, shouldPlaySound);
        }

        private void CheckEnter(GameObject target)
        {
            if (target.CompareTag("NoBuild"))
            {
                SetCanBePlaced(false);
                return;
            }

            if (target.CompareTag("ConnectingPoint") || target.CompareTag("ShelfZone") ||
                target.CompareTag("WorkBench")) return;
            if (_triggeredObjects.Contains(target)) return;
            _triggeredObjects.Add(target);
            CheckForAvailable();
        }

        private void CheckExit(GameObject target)
        {
            if (target.CompareTag("ConnectingPoint") || target.CompareTag("ShelfZone") ||
                target.CompareTag("WorkBench")) return;
            if (!_triggeredObjects.Contains(target)) return;
            _triggeredObjects.Remove(target);
            CheckForAvailable();
        }

        private void OnCollisionEnter(Collision other)
        {
            CheckEnter(other.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckEnter(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            CheckExit(other.gameObject);
        }

        private void OnCollisionExit(Collision other)
        {
            CheckExit(other.gameObject);
        }
    }
}