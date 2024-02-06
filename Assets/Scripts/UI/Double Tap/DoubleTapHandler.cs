using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Double_Tap
{
    public class DoubleTapHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler
    {
        [SerializeField]private UnityEvent _onSingleTap;
        [SerializeField] private UnityEvent _onDoubleTap;

        private float _firstTapTime = 0f;
        private float _timeBetweenTaps = 0.2f;
        private bool _doubleTapInitialized;
    
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_doubleTapInitialized)
            {
                Invoke("SingleTap", _timeBetweenTaps);
                _doubleTapInitialized = true;
                _firstTapTime = Time.time;
            }
            else if (Time.time - _firstTapTime < _timeBetweenTaps)
            {
                CancelInvoke("SingleTap");
                DoubleTap();
            }
        }

        private void SingleTap()
        {
            _doubleTapInitialized = false;
            if(_onSingleTap != null)
                _onSingleTap.Invoke();
        }

        private void DoubleTap()
        {
            _doubleTapInitialized = false;
            if(_onDoubleTap != null)
                _onDoubleTap.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CancelInvoke("SingleTap");
            _doubleTapInitialized = false;
        }
    }
}
