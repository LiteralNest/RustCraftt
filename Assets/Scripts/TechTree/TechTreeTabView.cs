using UnityEngine;
using UnityEngine.UI;

namespace TechTree
{
    public class TechTreeTabView : MonoBehaviour
    {
        [SerializeField] private GameObject _activeFon;
        [SerializeField] private GameObject _openningTab;
        [SerializeField] private Button _openningButton;
 
        private TechTreeTabsViewContainer _container;

        public void Init(TechTreeTabsViewContainer container)
        {
            _container = container;
            _openningButton.onClick.AddListener(OpenTab);
        }

        public void OpenTab()
        {
            _container.AssignTab(this);
            HandleTab(true);
        }
        
        public void CloseTab()
            => HandleTab(false);

        private void HandleTab(bool value)
        {
            _activeFon.SetActive(value);
            _openningTab.SetActive(value);
        }
    }
}