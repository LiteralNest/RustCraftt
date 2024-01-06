using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TechTree
{
    public class TechnologyInfoPanelUI : MonoBehaviour
    {
        [Header("Attached Scritps")] [SerializeField]
        private TechnologiesContainer _technologiesContainer;

        [Header("UI")] [SerializeField] private TMP_Text _techNameText;
        [SerializeField] private TMP_Text _techDescriptionText;
        [SerializeField] private TMP_Text _techCostText;
        [SerializeField] private Image _techImage;
        [SerializeField] private Button _researchButton;
        [SerializeField] private GameObject _infoPanel;

        private Technology _currentTechnology;

        private void OnEnable()
            => GlobalEventsContainer.InventoryDataChanged += SetResearchButton;

        private void OnDisable()
            => GlobalEventsContainer.InventoryDataChanged -= SetResearchButton;

        private void Start()
            => _researchButton.onClick.AddListener(ResearchTechnology);

        private void SetResearchButton()
            => _researchButton.interactable = !_currentTechnology.IsResearched && _currentTechnology.CanResearch();

        public void ShowTechnologyInfo(Technology technology, TechnologyUI technologyUI)
        {
            _technologiesContainer.DeselectTechnologies();
            technologyUI.Select(true);
            _currentTechnology = technology;
            _techImage.sprite = technology.Item.Icon;
            _techNameText.text = technology.Item.Name;
            _techDescriptionText.text = technology.Item.Description;
            _techCostText.text = "x" + _currentTechnology.Cost;
            SetResearchButton();
            _infoPanel.SetActive(true);
        }

        private void ResearchTechnology()
        {
            if(_currentTechnology == null) return;
            _currentTechnology.Research();
            _researchButton.interactable = false;
            _infoPanel.SetActive(false);
        }
    }
}