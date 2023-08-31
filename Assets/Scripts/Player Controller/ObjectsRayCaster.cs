using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectsRayCaster : MonoBehaviour
{
    [Header("UI")] [SerializeField] private GameObject _pointPanel;
    [SerializeField] private TMP_Text _obtainText;
    [SerializeField] private GameObject _lootButton;
    [SerializeField] private TMP_Text _lootButtonText;
    
    [Header("Distances")]
    [SerializeField] private float _maxOreHitDistance = 3f;
    [SerializeField] private float _maxGatheringDistance = 5f;
    [SerializeField] private float _maxLootBoxDistance = 5f;
    
    public ResourceOre TargetResourceOre { get; private set; }
    public GatheringOre TargetGathering { get; private set; }
    public LootBox TargetBox { get; private set; }

    private void FixedUpdate()
    {
        TryRaycastTargets();
    }

    private bool TryRaycast<T>(string tag, float hitDistance, out T target)
    {
        target = default;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, hitDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if(!hitObject.CompareTag(tag)) return false;
            target = hitObject.GetComponent<T>();
            return true;
        }

        return false;
    }
    

    private void SetLootText(string text, bool active = true)
    {
        _pointPanel.SetActive(!active);
        if (_obtainText.gameObject.activeSelf != active)
            _obtainText.gameObject.SetActive(active);
        _obtainText.text = text;
    }

    private void SetLootButton(string text, bool active = true)
    {
        _pointPanel.SetActive(!active);
        if(_lootButton.activeSelf != active)
            _lootButton.SetActive(active);
        _lootButtonText.text = text;
    }

    private bool OreReady(Ore ore)
        => !ore.Recovering;
    
    private void TryRaycastTargets()
    {
        SetLootText("", false);
        TargetBox = null;
        TargetGathering = null;
        TargetResourceOre = null;
        SetLootButton("", false);

        if (TryRaycast("Gathering", _maxGatheringDistance, out GatheringOre item))
        {
            if (OreReady(item))
            {
                TargetGathering = item;
                SetLootButton("Gather");
                return;
            }
        }

        if (TryRaycast("LootBox", _maxLootBoxDistance, out LootBox lootBox))
        {
            TargetBox = lootBox;
            SetLootButton("Open");
            return;
        }
        
        if (TryRaycast("Ore", _maxOreHitDistance, out ResourceOre ore))
        {
            if (OreReady(ore))
            {
                TargetResourceOre = ore;
                SetLootText("Obtain");
                return;
            }
        }
    }
}
