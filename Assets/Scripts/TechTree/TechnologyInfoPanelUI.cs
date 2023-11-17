using TMPro;
using UnityEngine;
using UnityEngine.UI;

   public class TechnologyInfoPanelUI : MonoBehaviour
   {
       [SerializeField] private TextMeshProUGUI _techNameText;
       [SerializeField] private TextMeshProUGUI _techDescriptionText;
       [SerializeField] private TextMeshProUGUI _techCostText;
       [SerializeField] private Button _researchButton;
       [SerializeField] private GameObject _infoPanel;

       private Technology _currentTechnology;

       private void Start()
       {
           _researchButton.onClick.AddListener(ResearchTechnology);
       }

       public void ShowTechnologyInfo(Technology technology)
       {
           _currentTechnology = technology;

           _techNameText.text = technology.TechName;
           _techDescriptionText.text = technology.TechDescription;
           _techCostText.text = "Cost: " + technology.Cost; 
           
           _researchButton.interactable = !technology.IsResearched && technology.CanResearch();

           _infoPanel.SetActive(true); 
       }

       private void ResearchTechnology()
       {
           if (_currentTechnology != null)
           {
               _currentTechnology.Research();
               _researchButton.interactable = false;
               _infoPanel.SetActive(false);
           }
       }
   }