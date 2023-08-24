using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerExitHandler ,IPointerClickHandler, IPointerDownHandler
{
    [SerializeField] private UnityEvent _pointerDown;
    [SerializeField] private UnityEvent _pointerClicked;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDown?.Invoke();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _pointerClicked?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerClicked?.Invoke();
    }
}
