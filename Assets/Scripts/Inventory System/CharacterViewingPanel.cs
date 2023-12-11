using UnityEngine;
using UnityEngine.EventSystems;


public class CharacterViewingPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 lastTouchPosition;
 
     [SerializeField]
     private float rotationSpeed = 0.1f;

     [SerializeField] private Transform _rotationTarget;

     public void OnPointerDown(PointerEventData eventData)
     {
         lastTouchPosition = eventData.position;
     }
 
     public void OnDrag(PointerEventData eventData)
     {
         var currentTouchPosition = eventData.position;
         float deltaX = currentTouchPosition.x - lastTouchPosition.x;
         _rotationTarget.Rotate(Vector3.up, -deltaX * rotationSpeed);
         lastTouchPosition = currentTouchPosition;
     }
}
