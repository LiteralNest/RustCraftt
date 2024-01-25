using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class CustomButton : MonoBehaviour, IPointerUpHandler ,IPointerClickHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private UnityEvent _pointerDown;
        [SerializeField] private UnityEvent _pointerClicked;
        [SerializeField] private UnityEvent _pointerClickedWithoudDisable;
    
        public UnityEvent PointerDown => _pointerDown;
        public UnityEvent PointerClicked => _pointerClicked;
        public UnityEvent PointerClickedWithoudDisable => _pointerClickedWithoudDisable;

        private void OnDisable()
        {
            _pointerClicked?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pointerDown?.Invoke();
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            _pointerClicked?.Invoke();
            _pointerClickedWithoudDisable?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pointerClicked?.Invoke();
            _pointerClickedWithoudDisable?.Invoke();
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerClicked?.Invoke();
            _pointerClickedWithoudDisable?.Invoke();
        }
    }
}
