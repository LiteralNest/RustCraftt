using UnityEngine;

namespace Map
{
    public class MapFingerScroller : MonoBehaviour
    {
        private bool _isDragging = false;
        private Vector2 _dragStartPosition;

        private void Update()
            => DetectFingerInput();

        void DetectFingerInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnDragBegin(touch.position);
                        break;

                    case TouchPhase.Moved:
                        OnDrag(touch.position);
                        break;

                    case TouchPhase.Ended:
                        OnDragEnd();
                        break;
                }
            }
        }

        void OnDragBegin(Vector2 startPosition)
        {
            _isDragging = true;
            _dragStartPosition = startPosition;
        }

        void OnDrag(Vector2 currentPosition)
        {
            if (_isDragging)
            {
                Vector2 dragDelta = currentPosition - _dragStartPosition;
                //scrollRect.normalizedPosition -= new Vector2(dragDelta.x / Screen.width, dragDelta.y / Screen.height);
                _dragStartPosition = currentPosition;
            }
        }

        void OnDragEnd()
        {
            _isDragging = false;
        }
    }
}