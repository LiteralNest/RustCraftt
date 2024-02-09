using System.Collections.Generic;
using Building_System.Upgrading.UI;
using Inventory_System;
using Player_Controller;
using UnityEngine;

namespace Building_System.Upgrading
{
    public class BuildingUpgrader : MonoBehaviour
    {
        [Header("Attached scripts")] [SerializeField]
        private BuildingUpgradeView buildingUpgradeView;

        [SerializeField] private List<BuildingUpgradeSlot> _buildingUpgradeSlots;

        [Header("Main Params")] [SerializeField]
        private LayerMask _targetMask;

        [SerializeField] private Camera _targetCamera;
        [SerializeField] private float _hammerRange = 5f;

        private int _selectedLevel = 1;
        public int SelectedLevel => _selectedLevel;

        private IHammerInteractable _hammerInteractable;

        private void OnDisable()
        {
            buildingUpgradeView.DisplayButtons(false, false, false, false);
            buildingUpgradeView.DisplayCycle(false);
        }

        private void Start()
        {
            if (_targetCamera == null)
                _targetCamera = Camera.main;
        }

        private void Update()
            => _hammerInteractable = TryRayCastUpgradable();

        private bool EnoughItems(IHammerInteractable hammerInteractable)
        {
            var cells = hammerInteractable.GetNeededCellsForUpgrade(_selectedLevel);
            return InventoryHandler.singleton.CharacterInventory.EnoughMaterials(cells);
        }

        private string GetUpgradeText()
        {
            foreach (var slot in _buildingUpgradeSlots)
                if (slot.Level == _selectedLevel)
                    return slot.UpgradeName;
            return "Error";
        }

        private IHammerInteractable TryRayCastUpgradable()
        {
            Vector3 rayOrigin = _targetCamera.transform.position;
            Vector3 rayDirection = _targetCamera.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, _hammerRange, _targetMask))
            {
                var upgradable = hit.transform.GetComponent<IHammerInteractable>();
                if (upgradable == null)
                {
                    buildingUpgradeView.DisplayButtons(false, false, false, false);
                    return null;
                }

                bool canBeUpgraded = upgradable.CanBeUpgraded(_selectedLevel) && EnoughItems(upgradable);

                if (upgradable.CanBeUpgraded(_selectedLevel))
                    PlayerNetCode.Singleton.UpgradeTextView.DisplayText(GetUpgradeText(),
                        upgradable.GetNeededCellsForUpgrade(_selectedLevel)[0]);
                else
                    PlayerNetCode.Singleton.UpgradeTextView.HandleText(false);

                buildingUpgradeView.DisplayButtons(canBeUpgraded, upgradable.CanBeDestroyed(),
                    upgradable.CanBeRepaired(), upgradable.CanBePickUp(),
                    upgradable.GetNeededCellsForUpgrade(_selectedLevel));
                return upgradable;
            }

            buildingUpgradeView.DisplayButtons(false, false, false, false);
            return null;
        }

        public void Upgrade()
        {
            if (_hammerInteractable == null) return;
            _hammerInteractable.UpgradeTo(_selectedLevel);
        }

        public void SetSelectedLevel(int value)
        {
            _selectedLevel = value;
            buildingUpgradeView.RedisplayCells();
        }

        public void Repair()
        {
            if (_hammerInteractable == null) return;
            _hammerInteractable.Repair();
        }

        public void Destroy()
        {
            if (_hammerInteractable == null) return;
            _hammerInteractable.Destroy();
        }

        public void PickUp()
        {
            if (_hammerInteractable == null) return;
            _hammerInteractable.PickUp();
        }
    }
}