using System;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Technology _technology;
    [SerializeField] private TechnologyInfoPanelUI _infoPanel;
    [SerializeField] private Button _iconButton;
    private void Awake()
    {
        _image.sprite = _technology.TechImage;
    }

    private void Start()
    {
        _iconButton.onClick.AddListener(() => _infoPanel.ShowTechnologyInfo(_technology));
    }
}