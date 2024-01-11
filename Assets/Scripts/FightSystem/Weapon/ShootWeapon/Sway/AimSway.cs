using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon.Sway
{
    public class AimSway : Sway
    {
        [Tooltip("Adjust how much the weapon sways")]
        [SerializeField] private float _swayAmount = 1f;
        [Tooltip("Maximum sway amount")]
        [SerializeField] private float _maxSwayAmount = 0.05f;
        [Tooltip("Horizontal sway bounds")]
        [SerializeField] private float _swayXBounds = 0.01f;
        [Tooltip("Smoothing factor for sway")]
        [SerializeField] private float _smoothFactor = 2f;
        [Tooltip("Return speed at previous position")]
        [SerializeField] private float _returnSpeed = 4f;

        private Vector3 _initialPosition;
        private Vector2 _previousTouchPosition;
        
        
        public  void AssingInitialPosition()
        {
            _initialPosition = SwayTransform.localPosition;
        }

        public override void UpdateSway()
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                if (touchDeltaPosition != Vector2.zero && touchDeltaPosition != _previousTouchPosition &&
                    touchDeltaPosition.magnitude > 0.1f)
                {
                    float swayX = Mathf.Clamp(-touchDeltaPosition.x * _swayAmount, -_maxSwayAmount, _maxSwayAmount) *
                                  _swayXBounds;

                    Vector3 swayPosition = _initialPosition + new Vector3(swayX, 0f, 0f);
                    SwayTransform.localPosition = Vector3.Lerp(SwayTransform.localPosition, swayPosition,
                        Time.deltaTime * _smoothFactor);
                }

                _previousTouchPosition = touchDeltaPosition;
            }
            else
            {
                SwayTransform.localPosition =
                    Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * _returnSpeed);
            }
        }
    }
}