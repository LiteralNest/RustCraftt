using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon.Sway
{
    public class WeaponSway : Sway
    {
        [Tooltip("Adjust how much the weapon sways")]
        [SerializeField] private float _swayAmount = 1f;
        [Tooltip("Maximum sway amount")]
        [SerializeField] private float _maxSwayAmount = 0.14f;
        [Tooltip("Horizontal sway bounds")]
        [SerializeField] private float _swayXBounds = 0.01f;
        [Tooltip("Vertical sway bounds")]
        [SerializeField] private float _swayYBounds = 0.01f;
        [Tooltip("Smoothing factor for sway")]
        [SerializeField] private float _smoothFactor = 1f;
        [Tooltip("Adjust how much weapon rotates")]
        [SerializeField] private float _rotationAmount = 3.5f;
        [Tooltip("Adjust how much the weapon sways")]
        [SerializeField] private float _maxRotation = 6.0f;
        [Tooltip("Return speed at previous position")]
        [SerializeField] private float _returnSpeed = 4f;
        
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector2 _previousTouchPosition;
        
        public override void Init(Transform swayTransform)
        {
            base.Init(swayTransform);
            _initialPosition = SwayTransform.localPosition;
            _initialRotation = SwayTransform.localRotation;
        }

        public override void UpdateSway()
        {
            if(!CanSway) return;
            if (Input.touchCount > 0)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                if (touchDeltaPosition != Vector2.zero && touchDeltaPosition != _previousTouchPosition &&  touchDeltaPosition.magnitude > 0.1f)
                {
                    float swayX = Mathf.Clamp(-touchDeltaPosition.x * _swayAmount, -_maxSwayAmount, _maxSwayAmount) * _swayXBounds;
                    float swayY = Mathf.Clamp(-touchDeltaPosition.y * _swayAmount, -_maxSwayAmount, _maxSwayAmount) * _swayYBounds;

                    Vector3 swayPosition = _initialPosition + new Vector3(swayX, swayY, 0f);
                    SwayTransform.localPosition = Vector3.Lerp(SwayTransform.localPosition, swayPosition, Time.deltaTime * _smoothFactor);

                    float rotationX = Mathf.Clamp(-touchDeltaPosition.y * _rotationAmount, -_maxRotation, _maxRotation);
                    float rotationY = Mathf.Clamp(touchDeltaPosition.x * _rotationAmount, -_maxRotation, _maxRotation);

                    Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0f) * _initialRotation;
                    SwayTransform.localRotation = Quaternion.Slerp(SwayTransform.localRotation, targetRotation, Time.deltaTime * _smoothFactor);
                }
            
                _previousTouchPosition = touchDeltaPosition;
            }
            else
            {
                SwayTransform.localPosition = Vector3.Lerp(SwayTransform.localPosition, _initialPosition, Time.deltaTime * _returnSpeed);
                SwayTransform.localRotation = Quaternion.Slerp(SwayTransform.localRotation, _initialRotation, Time.deltaTime * _returnSpeed);
            }
        }
    }
}
