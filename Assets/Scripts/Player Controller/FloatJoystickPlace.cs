using UnityEngine;
using UnityEngine.EventSystems;

namespace Player_Controller
{
    public class FloatJoystickPlace : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _targetJoystick;


        public void OnPointerDown(PointerEventData eventData)
        {
            _targetJoystick.SetActive(true);
            _targetJoystick.transform.position = eventData.position; 
        }
    }
}
