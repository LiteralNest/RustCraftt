using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingDragger))]
public class BuildingChooser : MonoBehaviour
{
    [Header("Attached scripts")]
    [SerializeField] private BuildingDragger _buildingDragger;
    
    [Header("UI")] [SerializeField] private GameObject _choosingPanel;

    private void Start()
    {
        if (_buildingDragger == null)
            _buildingDragger = GetComponent<BuildingDragger>();
    }

    public void ChooseBuilding(BuildingBluePrint buildingBluePrint)
    {
        var instance = Instantiate(buildingBluePrint);
        _buildingDragger.SetCurrentPref(instance);
        _choosingPanel.SetActive(false);
    }
}
