using UnityEngine;

namespace UI
{
    public class MainUiHandler : MonoBehaviour
    {
        public static MainUiHandler Singleton { get; private set; }

        [SerializeField] private GameObject _deathPanel;
        [SerializeField] private GameObject _knockDownPanel;
        
        private void Awake()
            => Singleton = this;
        
        [ContextMenu("DisplayDeathScreen")]
        public void DisplayDeathScreen(bool value)
            => _deathPanel.SetActive(value);
        
        [ContextMenu("DisplayKnockDownScreen")]
        public void DisplayKnockDownScreen(bool value)
            => _knockDownPanel.SetActive(value);
    }
}
