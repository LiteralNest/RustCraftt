using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon
{
    public class AimSway : MonoBehaviour
    {
        [Header("Adjust how much the weapon sways")]
        [SerializeField] private float _swayAmount = 1f;

        [Header("Maximum sway amount")]
        [SerializeField] private float _maxSwayAmount = 0.05f;

        [Header("Horizontal sway bounds")]
        [SerializeField] private float _swayXBounds = 0.01f;

        [Header("Smoothing factor for sway")]
        [SerializeField] private float _smoothFactor = 2f;

        private Vector3 _initialPosition;
        private Vector2 _previousTouchPosition;

        private void Start()
        {
            _initialPosition = transform.localPosition;
        }

        private void Update()
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
                    transform.localPosition = Vector3.Lerp(transform.localPosition, swayPosition,
                        Time.deltaTime * _smoothFactor);
                }

                _previousTouchPosition = touchDeltaPosition;
            }
            else
            {
                transform.localPosition =
                    Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * _smoothFactor);
            }
        }
    }
}