using System.Collections.Generic;
using Building_System.Building.Blocks;
using Events;
using Inventory_System;
using UnityEngine;

namespace Building_System.Blue_Prints
{
    public abstract class BluePrint : MonoBehaviour
    {
        [Header("Renderers")] public List<BuildingBluePrintCell> BluePrintCells = new List<BuildingBluePrintCell>();
        [Header("Layers")] [SerializeField] protected LayerMask _targetMask;
        [SerializeField] protected List<string> _placingTags = new List<string>();
        [field: SerializeField] public Vector3 StructureSize { get; private set; } = Vector3.one;
        protected bool _rotatedSide;

        #region Abstract

        public abstract void Place();

        public abstract void InitPlacedObject(BuildingStructure structure);

        #endregion

        private void OnEnable()
            => GlobalEventsContainer.InventoryDataChanged += CheckMaterials;

        private void OnDisable()
            => GlobalEventsContainer.InventoryDataChanged -= CheckMaterials;

        private void Start()
        {
            CheckMaterials();
        }

        private void CheckMaterials()
        {
            foreach (var cell in BluePrintCells)
                cell.EnoughMaterials = EnoughMaterials();
        }

        
        public virtual bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords, out Quaternion rotation,
            out bool shouldRotate, float distance)
        {
            shouldRotate = false;
            Vector3 rayOrigin = targetCamera.transform.position;
            Vector3 rayDirection = targetCamera.transform.forward;
            RaycastHit hit;
            rotation = default;
            coords = default;
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, distance, _targetMask))
            {
                if (!_placingTags.Contains(hit.collider.tag)) return false;
                
                int x, y, z;
                y = Mathf.RoundToInt(hit.point.y + hit.normal.y / 2);
                
                if (_rotatedSide)
                {
                    x = Mathf.RoundToInt(hit.point.x + hit.normal.x / 2);
                    z = Mathf.RoundToInt(hit.point.z + hit.normal.z / 2);
                }
                else
                {
                    x = Mathf.RoundToInt(hit.point.x + hit.normal.x / 2);
                    z = Mathf.RoundToInt(hit.point.z + hit.normal.z / 2);
                }
                
                coords = new Vector3(x, y, z);
                return true;
            }

            coords = default;
            return false;
        }

        public void Rotate()
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
            _rotatedSide = !_rotatedSide;
        }

        protected bool EnoughMaterials()
        {
            List<InventoryCell> cells = new List<InventoryCell>();
            foreach (var cell in BluePrintCells)
                cells.AddRange(cell.TargetBuildingStructure.GetPlacingRemovingCells());
            return InventoryHelper.EnoughMaterials(cells, InventoryHandler.singleton.CharacterInventory.ItemsNetData);
        }

        public void SetOnFrontOfPlayer(bool value)
        {
            foreach (var cell in BluePrintCells)
            {
                cell.OnFrontOfPlayer = value;
                cell.CheckForAvailable();
            }
        }
    }
}