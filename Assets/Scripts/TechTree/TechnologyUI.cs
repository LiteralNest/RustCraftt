using TechTree;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TechnologyUI : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private Image _techImage;
    [SerializeField] private TechnologyInfoPanelUI _infoPanel;
    [SerializeField] private Button _researchButton;
    [Space][Space]
    [SerializeField] private GameObject _lockedPanel;
    [SerializeField] private GameObject _selectedFon;
    [SerializeField] private GameObject _unlockedPanel;

    private Technology _technology;
    
    private void Start()
    {
        _researchButton.onClick.AddListener(() => _infoPanel.ShowTechnologyInfo(_technology, this));
    }

    public void DisplayTech(Technology tech)
    {
        _technology = tech;
        _techImage.sprite = tech.Item.Icon;
    }

    public void UnlockTech()
    {
        _lockedPanel.SetActive(false);
        _unlockedPanel.SetActive(true);
    }

    public void Select(bool value)
        => _selectedFon.SetActive(value);
}