using TMPro;
using UnityEngine;

public class ObjectsRayCaster : MonoBehaviour
{
    [Header("Attached scripts")] [SerializeField]
    private BuildingDataDisplayer _buildingDataDisplayer;
    
    [Header("UI")]
    [SerializeField] private GameObject _pointPanel;
    [SerializeField] private TMP_Text _obtainText;
    [SerializeField] private GameObject _lootButton;
    [SerializeField] private TMP_Text _lootButtonText;
    
    [Header("Distances")]
    [SerializeField] private float _maxOreHitDistance = 3f;
    [SerializeField] private float _maxGatheringDistance = 5f;
    [SerializeField] private float _maxLootBoxDistance = 5f;
    [SerializeField] private float _maxBlockHitDistance = 5f;

    [Header("Layers")] [SerializeField] private LayerMask _defaultMask;
    [SerializeField] private LayerMask _blockMask;
    
    public ResourceOre TargetResourceOre { get; private set; }
    public GatheringOre TargetGathering { get; private set; }
    public LootBox TargetBox { get; private set; }
    public LootingItem LootingItem { get; private set; }
    private BuildingBlock _targetBlock;

    private void FixedUpdate()
    {
        TryRaycastTargets();
    }

    private bool TryRaycast<T>(string tag, float hitDistance, out T target, LayerMask layer)
    {
        target = default;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, hitDistance, layer))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if(!hitObject.CompareTag(tag)) return false;
            if (!hitObject.TryGetComponent<T>(out target)) return false;
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
        _buildingDataDisplayer.DisableBuildingPanel();
        if (_targetBlock != null)
        {
            _targetBlock.CurrentBlock.TurnOutline(false);
            _targetBlock = null;
        }
        GlobalEventsContainer.GatherButtonActivated?.Invoke(false);
        GlobalEventsContainer.PickUpButtonActivated?.Invoke(false);
        SetLootText("", false);
        TargetBox = null;
        TargetGathering = null;
        TargetResourceOre = null;
        SetLootButton("", false);

        if(TryRaycast("LootingItem", _maxGatheringDistance, out LootingItem lootingItem, _defaultMask))
        {
            LootingItem = lootingItem;
            SetLootButton("Gather");
            GlobalEventsContainer.PickUpButtonActivated?.Invoke(true);
            return;
        }
        
        if(TryRaycast("Block", _maxBlockHitDistance, out BuildingBlock block, _blockMask))
        {
            if (block != null)
            {
                _targetBlock = block;
                _targetBlock.CurrentBlock.TurnOutline(true);
                _buildingDataDisplayer.DisplayBuildingData(_targetBlock);
            }
        }
        
        if (TryRaycast("Gathering", _maxGatheringDistance, out GatheringOre item, _defaultMask))
        {
            if (OreReady(item))
            {
                TargetGathering = item;
                SetLootButton("Gather");
                return;
            }
        }

        if (TryRaycast("LootBox", _maxLootBoxDistance, out LootBox lootBox, _defaultMask))
        {
            TargetBox = lootBox;
            SetLootButton("Open");
            return;
        }
        
        if (TryRaycast("Ore", _maxOreHitDistance, out ResourceOre ore, _defaultMask))
        {
            if (OreReady(ore))
            {
                GlobalEventsContainer.GatherButtonActivated?.Invoke(true);
                TargetResourceOre = ore;
                SetLootText("Obtain");
                return;
            }
        }
    }
}
