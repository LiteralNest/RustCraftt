using TechTree;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyUI : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private Image _fon;
    [SerializeField] private Image _techImage;
    [SerializeField] private TechnologyInfoPanelUI _infoPanel;
    [SerializeField] private Button _researchButton;
    [Space][Space]
    [SerializeField] private GameObject _lockedPanel;

    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private Color _researchedColor;

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
        _fon.color = _unselectedColor;
        _lockedPanel.SetActive(false);
    }

    public void Select(bool value)
    {
        if(value)
            _fon.color = _selectedColor;
        else
            _fon.color = _unselectedColor;
    }
}