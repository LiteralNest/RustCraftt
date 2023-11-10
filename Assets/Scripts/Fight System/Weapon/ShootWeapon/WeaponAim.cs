using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    public class WeaponAim : MonoBehaviour
    {
        [SerializeField] private Transform _weaponAimPosition;
        [SerializeField] private WeaponSway _sway;
        [SerializeField] private float _aimSpeed = 5f;

        private Transform _transform;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private bool _isAiming = false;
        private void OnGUI()
        {
            int buttonWidth = 150;
            int buttonHeight = 50;

            if (GUI.Button(new Rect((Screen.width - buttonWidth) / 2, (Screen.height - buttonHeight) / 2, buttonWidth, buttonHeight), "Set On Aim Position"))
            {
                SetOnAimPosition();
            }

            if (GUI.Button(new Rect((Screen.width - buttonWidth) / 2, (Screen.height - buttonHeight) / 2 + 100, buttonWidth, buttonHeight), "Set On Original Position"))
            {
                SetOnOriginalPosition();
            }
        }
            
        private void Awake()
        {
            _transform = transform;
            _originalPosition = _transform.localPosition;
            _originalRotation = _transform.localRotation;
        }

        private void SetOnAimPosition()
        {
            _transform.localPosition = _weaponAimPosition.localPosition;
            _transform.localRotation = _weaponAimPosition.localRotation;

            _sway.enabled = false;
        }

        private void SetOnOriginalPosition()
        {
            _transform.localPosition = _originalPosition;
            _transform.localRotation = _originalRotation;

            _sway.enabled = true;
        }
    }
}