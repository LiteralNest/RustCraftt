using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{
    public class MapMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private Camera _targetCamera;
        
        private bool isDragging = false;
        private Vector3 offset;

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            offset = _targetCamera.transform.position -_targetCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10.0f));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                Vector3 newPosition = _targetCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 10.0f)) + offset;
                _targetCamera.transform.position = -new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
        }
    }
}