using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapButton : MonoBehaviour
    {
        private Button _targetButton;

        public void Awake()
        {
            _targetButton = GetComponent<Button>();
            _targetButton.onClick.AddListener(Click);
        }

        private void Click()
            => GlobalEventsContainer.OnMapOpened?.Invoke();
    }
}