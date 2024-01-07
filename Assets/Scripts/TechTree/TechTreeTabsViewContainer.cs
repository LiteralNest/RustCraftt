using System.Collections.Generic;
using UnityEngine;

namespace TechTree
{
    public class TechTreeTabsViewContainer : MonoBehaviour
    {
        [SerializeField] private List<TechTreeTabView> _tabs;

        private TechTreeTabView _selectedTab;

        private void OnEnable()
            => _tabs[0].OpenTab();

        private void Awake()
        {
            foreach (var tab in _tabs)
                tab.Init(this);
        }

        public void AssignTab(TechTreeTabView tab)
        {
            if (_selectedTab != null)
                _selectedTab.CloseTab();

            _selectedTab = tab;
        }
    }
}