using Player_Controller;
using UnityEngine;

namespace InteractSystem
{
    public class RayCastInteract : MonoBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private RayCastInteractView _rayCastInteractView;

        [Header("Main Values")] [SerializeField]
        private Camera _targetCamera;

        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _layerMask;

        private IRaycastInteractable _target;

        private void Update()
        {
            TryRayCastDataDisplayable();
            TryRayCastInteractable();
        }

        private void TryRayCastInteractable()
        {
            if (!_targetCamera.gameObject.activeSelf) return;
            Ray ray = new Ray(_targetCamera.transform.position, _targetCamera.transform.forward);
            Debug.DrawRay(_targetCamera.transform.position, Camera.main.transform.forward * _maxDistance, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _layerMask))
                _target = hitInfo.collider.gameObject.GetComponent<IRaycastInteractable>();
            else
                _target = null;

            if (_target != null && _target.CanInteract())
                _rayCastInteractView.DisplayData(_target);
            else
                _rayCastInteractView.ClosePanel();
        }

        private void TryRayCastDataDisplayable()
        {
            if (!_targetCamera.gameObject.activeSelf) return;
            Ray ray = new Ray(_targetCamera.transform.position, _targetCamera.transform.forward);
            var targets = Physics.RaycastAll(ray, _maxDistance, _layerMask);
            foreach (var target in targets)
            {
                var dataDisplayable = target.collider.GetComponent<IRayCastHpDisplayer>();
                if (dataDisplayable != null)
                {
                    dataDisplayable.DisplayData();
                    return;
                }
            }
            if(PlayerNetCode.Singleton)
                PlayerNetCode.Singleton.ObjectHpDisplayer.DisablePanel();
        }
    }
}