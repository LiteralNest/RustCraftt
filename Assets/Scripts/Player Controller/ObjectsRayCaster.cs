using Building_System;
using Building_System.Blocks;
using Crafting_System.WorkBench;
using FightSystem.Damage;
using FightSystem.Weapon.Explosive;
using Items_System;
using MeltingSystem;
using PlayerDeathSystem;
using ResourceOresSystem;
using RespawnSystem.SleepingBag;
using Storage_System;
using TMPro;
using Tool_Clipboard;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicle;
using Vehicle.SittingPlaces;
using Web.UserData;

namespace Player_Controller
{
    public class ObjectsRayCaster : MonoBehaviour
    {
        [FormerlySerializedAs("_buildingDataDisplayer")] [Header("Attached scripts")] [SerializeField]
        private ObjectHpDisplayer objectHpDisplayer;

        [SerializeField] private VehiclesController _vehiclesController;


        [SerializeField] private PlayerNetCode _playerNetCode;
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
        [SerializeField] private LayerMask _playerMask;
        [SerializeField] private LayerMask _resourceOreMask;
    
        public GatheringOre TargetGathering { get; private set; }
        public Storage TargetBox { get; private set; }
        public LootingItem LootingItem { get; private set; }
        public Smelter Smelter { get; private set; }
        public Recycler.Recycler RecyclerHandler { get; private set; }
        public DoorHandler DoorHandler { get; private set; }
        public ToolClipboard ToolClipboard { get; private set; }
        public WorkBench WorkBench { get; private set; }
        public HammerInteractable TargetClipboardInteractable { get; private set; }
        public SittingPlace TargetSittingPlace { get; private set; }
        public SleepingBagNamer TargetSleepingBag { get; private set; }
        public PlayerKnockDowner PlayerKnockDowner { get; private set; }
        public SatchelExplosive SatchelExplosive { get; private set; }
        private BuildingBlock _targetBlock;

        public Vector3 LastRaycastedPosition { get; private set; }
        public Vector3 LastRayCastedRotation { get; private set; }

        private void Update()
        {
            TryRaycastTargets();
        }

        private void ResetTargets()
        {
            TargetGathering = null;
            TargetBox = null;
            LootingItem = null;
            Smelter = null;
            RecyclerHandler = null;
            DoorHandler = null;
            ToolClipboard = null;
            WorkBench = null;
            TargetClipboardInteractable = null;
            TargetSittingPlace = null;
            TargetSleepingBag = null;
            PlayerKnockDowner = null;
            SatchelExplosive = null;
            _vehiclesController.SetVehicleController(null);
        }

        private bool TryRaycast<T>(string tag, float hitDistance, out T target, LayerMask layer, bool ignoreTag = false)
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
                if (!ignoreTag)
                    if (!hitObject.CompareTag(tag))
                        return false;
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

        private void TryDisplayHp()
        {
            if (!TryRaycast("DamagingItem", _maxGatheringDistance, out IDamagable damagable, _defaultMask, true)) return;
            objectHpDisplayer.DisplayObjectHp(damagable);
        }

        private void TryRaycastTargets()
        {
            objectHpDisplayer.DisableBuildingPanel();
            if (_targetBlock != null)
            {
                _targetBlock = null;
            }

            SetLootText("", false);

            SetLootButton("", false);
            ResetTargets();
            TryDisplayHp();

            if (TryRaycast("Satchel", _maxOpeningDistance, out SatchelExplosive satchelExplosive, _defaultMask))
            {
                if (satchelExplosive.TurnedOff)
                {
                    SatchelExplosive = satchelExplosive;
                    SetLootButton("Fire");
                    return;
                }
            }

            if (TryRaycast("LootingItem", _maxGatheringDistance, out LootingItem lootingItem, _defaultMask))
            {
                LootingItem = lootingItem;
                SetLootButton("Gather");
                return;
            }

            if (TryRaycast("SleepingBag", _maxOpeningDistance, out SleepingBagNamer sleepingBag, _defaultMask))
            {
                TargetSleepingBag = sleepingBag;
                SetLootButton("Rename");
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

            if (TryRaycast("ClipBoardIteractable", _maxOpeningDistance, out HammerInteractable clipboardInteractable,
                    _blockMask))
            {
                TargetClipboardInteractable = clipboardInteractable;
            }

            if (TryRaycast("Gathering", _maxGatheringDistance, out GatheringOre item, _resourceOreMask))
            {
                TargetGathering = item;
                SetLootButton("Gather");
                return;
            }

            if (TryRaycast("LootBox", _maxOpeningDistance, out ToolClipboard clipboard, _defaultMask))
            {
                ToolClipboard = clipboard;
                if (!clipboard.IsAutorized(UserDataHandler.Singleton.UserData.Id))
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
                if(doorHandler.IsOpened)
                    SetLootButton("Close");
                else
                    SetLootButton("Open");
                return;
            }

            if (TryRaycast("WorkBench", _maxOpeningDistance, out WorkBench workBench, _defaultMask))
            {
                WorkBench = workBench;
                SetLootButton("Open");
                return;
            }

            if (TryRaycast("SitPlace", _maxOpeningDistance, out SittingPlace place, _defaultMask))
            {
                TargetSittingPlace = place;
                if (place.CanSit())
                    SetLootButton("Sit");
                else if (place.CanStand(_playerNetCode))
                    SetLootButton("Stand");
                return;
            }

            if (TryRaycast("Vehicle", _maxOpeningDistance, out VehicleController vehicle, _defaultMask))
            {
                _vehiclesController.SetVehicleController(vehicle);
                return;
            }

            if (TryRaycast("Player", _maxOpeningDistance, out PlayerKnockDowner player, _playerMask))
            {
                if (player.IsKnockDown && player.OwnerClientId != PlayerNetCode.Singleton.OwnerClientId)
                {
                    PlayerKnockDowner = player;
                    SetLootButton("Help");
                    return;
                }
            }
        }
    }
}