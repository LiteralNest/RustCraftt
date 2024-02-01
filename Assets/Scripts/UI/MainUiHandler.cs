using UnityEngine;

namespace UI
{
    public class MainUiHandler : MonoBehaviour
    {
        public static MainUiHandler Singleton { get; private set; }

        [SerializeField] private GameObject _deathPanel;
        [SerializeField] private GameObject _knockDownPanel;
        [SerializeField] private GameObject _mapClose;

        private void Awake()
            => Singleton = this;

        [ContextMenu("DisplayDeathScreen")]
        public void DisplayDeathScreen(bool value)
        {
            _mapClose.SetActive(false);
            _deathPanel.SetActive(value);
            
        }

        [ContextMenu("DisplayKnockDownScreen")]
        public void DisplayKnockDownScreen(bool value)
        {
            _mapClose.SetActive(false);
            _knockDownPanel.SetActive(value);
        }
    }
}