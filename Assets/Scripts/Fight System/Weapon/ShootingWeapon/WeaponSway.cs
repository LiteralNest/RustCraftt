using UnityEngine;

namespace Fight_System.Weapon.ShootingWeapon
{
    public class WeaponSway : MonoBehaviour
    {
        [SerializeField] private float _swayAmount = 1f;    // Adjust how much the weapon sways
        [SerializeField] private float _maxSwayAmount = 0.14f; // Maximum sway amount
        [SerializeField] private float _swayXBounds = 0.01f;    // Horizontal sway bounds
        [SerializeField] private float _swayYBounds = 0.01f;    // Vertical sway bounds
        [SerializeField] private float _smoothFactor = 1f;   // Smoothing factor for sway
        [SerializeField] private float _rotationAmount = 3.5f; // Adjust how much the weapon rotates
        [SerializeField] private float _maxRotation = 6.0f;    // Maximum rotation amount
        [SerializeField] private float _returnSpeed = 4f;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector2 _previousTouchPosition;
        private void Start()
        {
            _initialPosition = transform.localPosition;
            _initialRotation = transform.localRotation;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                if (touchDeltaPosition != Vector2.zero && touchDeltaPosition != _previousTouchPosition &&  touchDeltaPosition.magnitude > 0.1f)
                {
                    float swayX = Mathf.Clamp(-touchDeltaPosition.x * _swayAmount, -_maxSwayAmount, _maxSwayAmount) * _swayXBounds;
                    float swayY = Mathf.Clamp(-touchDeltaPosition.y * _swayAmount, -_maxSwayAmount, _maxSwayAmount) * _swayYBounds;

                    Vector3 swayPosition = _initialPosition + new Vector3(swayX, swayY, 0f);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, swayPosition, Time.deltaTime * _smoothFactor);

                    float rotationX = Mathf.Clamp(-touchDeltaPosition.y * _rotationAmount, -_maxRotation, _maxRotation);
                    float rotationY = Mathf.Clamp(touchDeltaPosition.x * _rotationAmount, -_maxRotation, _maxRotation);

                    Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0f) * _initialRotation;
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * _smoothFactor);
                }
            
                _previousTouchPosition = touchDeltaPosition;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * _returnSpeed);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, _initialRotation, Time.deltaTime * _returnSpeed);
            }
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            // Draw sway bounds
            Vector3 position = transform.position;
            Vector3 leftBound = position - transform.right * _swayXBounds;
            Vector3 rightBound = position + transform.right * _swayXBounds;
            Vector3 upperBound = position + transform.up * _swayYBounds;
            Vector3 lowerBound = position - transform.up * _swayYBounds;

            Gizmos.DrawLine(leftBound, rightBound);
            Gizmos.DrawLine(upperBound, lowerBound);
        }
    }
}
