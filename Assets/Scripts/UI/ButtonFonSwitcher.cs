using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonFonSwitcher : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private List<GameObject> _gameObjectsToToggle;

        private void Start()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                var gameObjectToToggle = _gameObjectsToToggle[i];
                var button = _buttons[i];

                button.onClick.AddListener(() => ToggleGameObject(gameObjectToToggle));
            }
        }

        private void ToggleGameObject(GameObject gameObjectToToggle)
        {
            foreach (var obj in _gameObjectsToToggle)
            {
                obj.SetActive(obj == gameObjectToToggle);
            }
        }
    }
}