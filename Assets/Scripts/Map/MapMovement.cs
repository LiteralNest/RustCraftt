using UnityEngine;

namespace Map
{
    public class MapMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float scrollSpeed = 2f;

        private void Update()
        {
            HandleCameraMovement();
            HandlePinchZoom();
        }

        private void HandleCameraMovement()
        {
            // Move the camera with one finger swipe
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                transform.Translate(-touchDeltaPosition.x * moveSpeed * Time.deltaTime, 0f,
                    -touchDeltaPosition.y * moveSpeed * Time.deltaTime);
            }
        }

        private void HandlePinchZoom()
        {
            // Zoom the camera with two fingers pinch
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Camera.main.fieldOfView += difference * scrollSpeed;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 80f);
            }
        }
    }
}