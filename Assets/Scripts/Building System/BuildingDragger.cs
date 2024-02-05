using Building_System.Blue_Prints;
using Events;
using UI;
using UnityEngine;

namespace Building_System
{
    public class BuildingDragger : MonoBehaviour
    {
        [SerializeField] private Camera _targetCamera;
        [SerializeField] private float _forwardPlacingOffset = 5f;
        [SerializeField] private float _rayCastDistance = 10f;
        [SerializeField] private LayerMask _noBuildMask;
    
        private BluePrint _currentPref;

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
            if (_currentPref == null) return;
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
            if (!_currentPref.TryGetObjectCoords(_targetCamera, out var coords, out var rotation, out bool shouldRotate, _rayCastDistance) || !CanBuild())
            {
                _currentPref.SetOnFrontOfPlayer(true);
                _currentPref.transform.position = GetFrontOfCameraPosition();
                return;
            }
            _currentPref.SetOnFrontOfPlayer(false);
            _currentPref.transform.position = coords;
            if(!shouldRotate) return;
            _currentPref.transform.rotation = rotation;
        }

        public void Place()
        {
            if (_currentPref == null) return;
            _currentPref.Place();
        }

        public void ClearCurrentPref()
        {
            if(_currentPref == null) return;
            Destroy(_currentPref.gameObject);
            CharacterUIHandler.singleton.ActivateBuildingStaffPanel(false);
            _currentPref = null;
        }
    
        public void SetCurrentPref(BluePrint target)
        {
            ClearCurrentPref();
            _currentPref = target;
        }

        public void Rotate()
        {
            if(_currentPref == null) return;
            _currentPref.Rotate();
        }
    }
}