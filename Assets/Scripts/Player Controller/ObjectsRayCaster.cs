using Building_System;
using Building_System.Blocks;
using FightSystem.Damage;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicle;

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
        
        public HammerInteractable TargetClipboardInteractable { get; private set; }
        private BuildingBlock _targetBlock;

        public Vector3 LastRaycastedPosition { get; private set; }
        public Vector3 LastRayCastedRotation { get; private set; }

        private void Update()
        {
            TryRaycastTargets();
        }

        private void ResetTargets()
        {
            TargetClipboardInteractable = null;
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
            
            if (TryRaycast("Vehicle", _maxOpeningDistance, out VehicleController vehicle, _defaultMask))
            {
                _vehiclesController.SetVehicleController(vehicle);
                return;
            }
        }
    }
}