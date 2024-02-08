using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class CharacterUIHandler : MonoBehaviour
    {
        public static CharacterUIHandler singleton { get; private set; }

        [SerializeField] private GameObject _hitUi;
        [SerializeField] private GameObject _buildingStaffPanel;
        [SerializeField] private GameObject _buildingButton;
        [SerializeField] private GameObject _buildingChoosingPanel;
        [SerializeField] private GameObject _placingPanel;
        [SerializeField] private GameObject _joystick;
        [SerializeField] private GameObject _moveUpButton;
        [SerializeField] private GameObject _sitAndJumpPanel;

        [SerializeField] private List<GameObject> _vehicleIgnoringPanels = new List<GameObject>();

        [SerializeField] private List<GameObject> _movingHudPanels = new List<GameObject>();

        [SerializeField] private GameObject _mainHudPanel;

        private void OnEnable()
        {
            GlobalEventsContainer.OnMainHudHandle += HandleHud;
            GlobalEventsContainer.OnPlayerKnockDown += DeactivateHud;
            GlobalEventsContainer.OnPlayerStandUp += ActivateHud;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.OnMainHudHandle -= HandleHud;
            GlobalEventsContainer.OnPlayerKnockDown -= DeactivateHud;
            GlobalEventsContainer.OnPlayerStandUp -= ActivateHud;
        }

        private void HandleHud(bool value)
            => _mainHudPanel.SetActive(value);

        private void ActivateHud()
            => _mainHudPanel.SetActive(true);
        
        private void DeactivateHud()
            => _mainHudPanel.SetActive(false);

        public void AssignSingleton()
            => singleton = this;

        public void ActivateBuildingStaffPanel(bool value)
            => _buildingStaffPanel.SetActive(value);

        public void ActivateBuildingChoosingPanel(bool value)
            => _buildingChoosingPanel.SetActive(value);

        public void ActivateBuildingButton(bool value)
            => _buildingButton.SetActive(value);

        public void ActivatePlacingPanel(bool value)
            => _placingPanel.SetActive(value);

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

        public void HandleMovingUpButton(bool value)
        {
            _moveUpButton.SetActive(value);
            _sitAndJumpPanel.SetActive(!value);
        }

        public IEnumerator DisplayHitRoutine()
        {
            _hitUi.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            _hitUi.SetActive(false);
        }
    }
}