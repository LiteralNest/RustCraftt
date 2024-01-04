using UnityEngine;

public class CameraMapScroller : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 5f;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Moved) return;
            var yOffset = touch.deltaPosition.y * Time.deltaTime * _scrollSpeed;
            var xOffset = touch.deltaPosition.x * Time.deltaTime * _scrollSpeed;

            transform.Translate(xOffset, yOffset, 0f);
        }
    }
}