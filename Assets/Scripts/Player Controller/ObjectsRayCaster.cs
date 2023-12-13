using Building_System.Blocks;
using Crafting_System.WorkBench;
using Items_System;
using Items_System.Ore_Type;
using MeltingSystem;
using Storage_System;
using TMPro;
using Tool_Clipboard;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicle;
using Web.User;

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
    [SerializeField] private VehiclesController _vehiclesController;
    public ResourceOre TargetResourceOre { get; private set; }
    public GatheringOre TargetGathering { get; private set; }
    public Storage TargetBox { get; private set; }
    public LootingItem LootingItem { get; private set; }
    public Smelter Smelter { get; private set; }
    public Recycler.Recycler RecyclerHandler { get; private set; }
    public DoorHandler DoorHandler { get; private set; }
    public ToolClipboard ToolClipboard { get; private set; }
    public WorkBench WorkBench { get; private set; }
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
        _vehiclesController.SetVehicleController(null);
        TargetResourceOre = null;
        TargetGathering = null;
        TargetBox = null;
        LootingItem = null;
        Smelter = null;
        RecyclerHandler = null;
        DoorHandler = null;
        ToolClipboard = null;
        WorkBench = null;
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
                CharacterUIHandler.singleton.ActivateGatherButton(true);
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
            _targetBlock = null;
        }

        if(CharacterUIHandler.singleton != null)
            CharacterUIHandler.singleton.ActivateGatherButton(false);
        SetLootText("", false);

        SetLootButton("", false);
        ResetTargets();
        TryDisplayHp();

        if (TryRaycast("LootingItem", _maxGatheringDistance, out LootingItem lootingItem, _defaultMask))
        {
            LootingItem = lootingItem;
            SetLootButton("Gather");
            return;
        }

        if (TryRaycast("Block", _maxBlockHitDistance, out BuildingBlock block, _blockMask))
        {
            if (block != null)
            {
                _targetBlock = block;
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
        
        if (TryRaycast("LootBox", _maxOpeningDistance, out ToolClipboard clipboard, _defaultMask))
        {
            ToolClipboard = clipboard;
            if (!clipboard.IsAutorized(UserDataHandler.singleton.UserData.Id))
            {
                SetLootButton("Authorize");
                return;
            }
        }
        
        if (TryRaycast("LootBox", _maxOpeningDistance, out Storage lootbox, _defaultMask))
        {
            TargetBox = lootbox;
            SetLootButton("Open");
            return;
        }

        if (TryRaycast("CampFire", _maxOpeningDistance, out Smelter campFireHandler, _defaultMask))
        {
            Smelter = campFireHandler;
            SetLootButton("Open");
            return;
        }

        if (TryRaycast("Recycler", _maxOpeningDistance, out Recycler.Recycler recycler, _defaultMask))
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

        if(TryRaycast("Vehicle", _maxOpeningDistance, out IVehicleController vehicleHandler, _defaultMask))
        {
            _vehiclesController.SetVehicleController(vehicleHandler);
            return;
        }
        
        if(TryRaycast("Boat", _maxOpeningDistance, out IVehicleController boatHandler, _defaultMask))
        {
            _vehiclesController.SetVehicleController(boatHandler);
            return;
        }
        
        if(TryRaycast("WorkBench", _maxOpeningDistance, out WorkBench workBench, _defaultMask))
        {
            WorkBench = workBench;
            SetLootButton("Open");
            return;
        }
        
        TryRayCastOre();
    }
}