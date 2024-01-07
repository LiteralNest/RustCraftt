using System.Collections.Generic;
using Events;
using UnityEngine;

namespace UI
{
    public class CharacterUIHandler : MonoBehaviour
    {
        public static CharacterUIHandler singleton { get; private set; }

        [SerializeField] private GameObject _buildingStaffPanel;
        [Space] [Space] [SerializeField] private GameObject _attackButton;
        [SerializeField] private GameObject _gatherButton;
        [SerializeField] private GameObject _buildingButton;
        [SerializeField] private GameObject _buildingChoosingPanel;
        [SerializeField] private GameObject _reloadingButton;
        [SerializeField] private GameObject _placingPanel;
        [SerializeField] private GameObject _scopeButton;
        [SerializeField] private GameObject _joystick;

        [SerializeField] private List<GameObject> _vehicleIgnoringPanels = new List<GameObject>();

        [SerializeField] private GameObject _meleeThrowButton;
        [SerializeField] private GameObject _throwExplosiveButton;

        [SerializeField] private List<GameObject> _movingHudPanels = new List<GameObject>();

        [SerializeField] private GameObject _mainHudPanel;

        private void OnEnable()
            => GlobalEventsContainer.OnMainHudHandle += HandleHud;

        private void OnDisable()
            => GlobalEventsContainer.OnMainHudHandle -= HandleHud;

        private void HandleHud(bool value)
            => _mainHudPanel.SetActive(value);

        public void AssignSingleton()
            => singleton = this;

        public void ActivateMeleeThrowButton(bool value)
            => _meleeThrowButton.SetActive(value);


        public void ActivateScope(bool value)
            => _scopeButton.SetActive(value);

        public void ActivateBuildingStaffPanel(bool value)
            => _buildingStaffPanel.SetActive(value);

        public void ActivateBuildingChoosingPanel(bool value)
            => _buildingChoosingPanel.SetActive(value);

        public void ActivateBuildingButton(bool value)
            => _buildingButton.SetActive(value);

        public void ActivateGatherButton(bool value)
            => _gatherButton.SetActive(value);

        public void ActivateAttackButton(bool value)
            => _attackButton.SetActive(value);

        public void ActivateReloadingButton(bool value)
            => _reloadingButton.SetActive(value);

        public void ActivatePlacingPanel(bool value)
            => _placingPanel.SetActive(value);

        public void ActivateThrowButton(bool value)
            => _throwExplosiveButton.SetActive(value);

        public void HandleJoystick(bool value)
            => _joystick.SetActive(value);

        public void HandleIgnoringVehiclePanels(bool value)
        {
            foreach (var panel in _vehicleIgnoringPanels)
                panel.SetActive(value);
        }

        public void HandleMovingHudPanels(bool value)
        {
            foreach (var panel in _movingHudPanels)
                panel.SetActive(value);
        }
    }
}