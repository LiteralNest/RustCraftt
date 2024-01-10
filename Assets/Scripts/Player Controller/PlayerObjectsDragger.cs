using Building_System.Placing_Objects;
using Events;
using UI;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerObjectsDragger : MonoBehaviour
    {
        [SerializeField] private Camera _targetCamera;
        [SerializeField] private float _forwardPlacingOffset = 5f;
        [SerializeField] private float _rayCastDistance = 10f;

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

        private void TryMoveBuildingObject()
        {
            if (!_targetBluePrint.TryGetObjectCoords(_targetCamera, out var coords, out var rotation, out var shouldRotate, _rayCastDistance))
            {
                _targetBluePrint.SetOnFrontOfPlayer(true);
                _targetBluePrint.transform.position = GetFrontOfCameraPosition();
                return;
            }

            _targetBluePrint.SetOnFrontOfPlayer(false);
            _targetBluePrint.transform.position = coords;
            if(!shouldRotate) return;
            _targetBluePrint.transform.rotation = rotation;
        }

        public void Place()
        {
            if (_targetBluePrint == null) return;
            _targetBluePrint.Place();
            CharacterUIHandler.singleton.ActivatePlacingPanel(false);
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