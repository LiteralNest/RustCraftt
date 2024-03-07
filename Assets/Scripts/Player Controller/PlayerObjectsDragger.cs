using Building_System.Building.Placing_Objects;
using Events;
using Inventory_System;
using UI;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerObjectsDragger : MonoBehaviour
    {
        [SerializeField] private Camera _targetCamera;
        [SerializeField] private float _forwardPlacingOffset = 5f;
        [SerializeField] private float _rayCastDistance = 10f;
        [SerializeField] private LayerMask _noBuildMask;
        
        private PlacingObjectBluePrint _targetBluePrint;

        private void OnEnable()
        {
            GlobalEventsContainer.BluePrintDeactivated += ClearCurrentPref;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.BluePrintDeactivated -= ClearCurrentPref;
        }

        private void Update()
        {
            if (_targetBluePrint == null) return;
            TryMoveBuildingObject();
        }

        private Vector3 GetFrontOfCameraPosition()
            => _targetCamera.transform.position + _targetCamera.transform.forward * _forwardPlacingOffset;

        private bool CanBuild()
        {
            var ray = new Ray(_targetCamera.transform.position, _targetCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, _rayCastDistance, _noBuildMask))
                return !(hit.collider.CompareTag("NoBuild"));
            return true;
        }
        
        private void TryMoveBuildingObject()
        {
            if (!_targetBluePrint.TryGetObjectCoords(_targetCamera, out var coords, out var rotation,
                    out var shouldRotate, _rayCastDistance) || !CanBuild())
            {
                _targetBluePrint.SetOnFrontOfPlayer(true);
                _targetBluePrint.transform.position = GetFrontOfCameraPosition();
                return;
            }

            _targetBluePrint.SetOnFrontOfPlayer(false);
            _targetBluePrint.transform.position = coords;
            if (!shouldRotate) return;
            _targetBluePrint.transform.rotation = rotation;
        }

        public void Place()
        {
            if (_targetBluePrint == null) return;
         
            var slot = InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell;
            bool shouldClearPref = false;
            
            if (slot.Count == 1)
            {
                GlobalEventsContainer.OnActiveSlotReset?.Invoke();
                CharacterUIHandler.singleton.ActivatePlacingPanel(false);
                shouldClearPref = true;
            }
            _targetBluePrint.Place();
            if(shouldClearPref)
                ClearCurrentPref();
        }

        public void ClearCurrentPref()
        {
            if (_targetBluePrint == null) return;
            Destroy(_targetBluePrint.gameObject);
            CharacterUIHandler.singleton.ActivatePlacingPanel(false);
            _targetBluePrint = null;
        }

        public void SetCurrentPref(PlacingObjectBluePrint target)
        {
            ClearCurrentPref();
            _targetBluePrint = Instantiate(target);
        }

        public void Rotate()
        {
            if (_targetBluePrint == null) return;
            _targetBluePrint.Rotate();
        }

        public void Reset()
        {
            ClearCurrentPref();
            CharacterUIHandler.singleton.ActivateBuildingStaffPanel(false);
            CharacterUIHandler.singleton.ActivatePlacingPanel(false);
        }
    }
}