using Storage_System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectsRayCaster : MonoBehaviour
{
    [FormerlySerializedAs("_buildingDataDisplayer")] [Header("Attached scripts")] [SerializeField]
    private ObjectHpDisplayer objectHpDisplayer;

    [Header("UI")] [SerializeField] private GameObject _pointPanel;
    [SerializeField] private TMP_Text _obtainText;
    [SerializeField] private GameObject _lootButton;
    [SerializeField] private TMP_Text _lootButtonText;

    [Header("Distances")] [SerializeField] private float _maxOreHitDistance = 3f;
    [SerializeField] private float _maxOpeningDistance = 5f;
    [SerializeField] private float _maxBlockHitDistance = 5f;
    [SerializeField] private float _maxGatheringDistance = 5f;

    [Header("Layers")] [SerializeField] private LayerMask _defaultMask;
    [SerializeField] private LayerMask _blockMask;

    public ResourceOre TargetResourceOre { get; private set; }
    public GatheringOre TargetGathering { get; private set; }
    public Storage TargetBox { get; private set; }
    public LootingItem LootingItem { get; private set; }
    public CampFireHandler CampFireHandler { get; private set; }
    public Recycler RecyclerHandler { get; private set; }
    public DoorHandler DoorHandler { get; private set; }
    private BuildingBlock _targetBlock;
    public bool CanRayCastOre { get; set; }

    public Vector3 LastRaycastedPosition { get; private set; }
    public Vector3 LastRayCastedRotation { get; private set; }

    private void Update()
    {
        TryRaycastTargets();
    }

    private void ResetTargets()
    {
        TargetResourceOre = null;
        TargetGathering = null;
        TargetBox = null;
        LootingItem = null;
        CampFireHandler = null;
        RecyclerHandler = null;
        DoorHandler = null;
    }

    private bool TryRaycast<T>(string tag, float hitDistance, out T target, LayerMask layer)
    {
        target = default;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hitDistance, Color.red);
        if (Physics.Raycast(ray, out hitInfo, hitDistance, layer))
        {
            LastRaycastedPosition = hitInfo.point;
            LastRayCastedRotation = hitInfo.normal;
            GameObject hitObject = hitInfo.collider.gameObject;
            if (!hitObject.CompareTag(tag)) return false;
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
        if (_lootButton.activeSelf != active)
            _lootButton.SetActive(active);
        _lootButtonText.text = text;
    }

    private bool OreReady(Ore ore)
        => !ore.Recovering;

    private void TryRayCastOre()
    {
        if (!CanRayCastOre) return;
        if (TryRaycast("Ore", _maxOreHitDistance, out ResourceOre ore, _defaultMask))
        {
            if (OreReady(ore))
            {
                MainUiHandler.singleton.ActivateGatherButton(true);
                TargetResourceOre = ore;
                SetLootText("Obtain");
                return;
            }
        }
    }

    private void TryDisplayHp()
    {
        if (!TryRaycast("DamagingItem", _maxGatheringDistance, out IDamagable damagable, _defaultMask)) return;
        objectHpDisplayer.DisplayObjectHp(damagable);
    }

    private void TryRaycastTargets()
    {
        objectHpDisplayer.DisableBuildingPanel();
        if (_targetBlock != null)
        {
            _targetBlock.CurrentBlock.TurnOutline(false);
            _targetBlock = null;
        }

        MainUiHandler.singleton.ActivateGatherButton(false);
        MainUiHandler.singleton.ActivatePickupButton(false);
        SetLootText("", false);

        SetLootButton("", false);
        ResetTargets();
        TryDisplayHp();

        if (TryRaycast("LootingItem", _maxGatheringDistance, out LootingItem lootingItem, _defaultMask))
        {
            LootingItem = lootingItem;
            SetLootButton("Gather");
            MainUiHandler.singleton.ActivatePickupButton(true);
            return;
        }

        if (TryRaycast("Block", _maxBlockHitDistance, out BuildingBlock block, _blockMask))
        {
            if (block != null)
            {
                _targetBlock = block;
                _targetBlock.CurrentBlock.TurnOutline(true);
                objectHpDisplayer.DisplayObjectHp(_targetBlock);
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

        if (TryRaycast("LootBox", _maxOpeningDistance, out Storage lootbox, _defaultMask))
        {
            TargetBox = lootbox;
            SetLootButton("Open");
            return;
        }

        if (TryRaycast("CampFire", _maxOpeningDistance, out CampFireHandler campFireHandler, _defaultMask))
        {
            CampFireHandler = campFireHandler;
            SetLootButton("Open");
            return;
        }

        if (TryRaycast("Recycler", _maxOpeningDistance, out Recycler recycler, _defaultMask))
        {
            RecyclerHandler = recycler;
            SetLootButton("Open");
            return;
        }

        if (TryRaycast("Door", _maxOpeningDistance, out DoorHandler doorHandler, _defaultMask))
        {
            DoorHandler = doorHandler;
            SetLootButton("Open");
            return;
        }

        TryRayCastOre();
    }
}