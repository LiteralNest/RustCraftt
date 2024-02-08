using InHandItems.InHandViewSystem;
using Inventory_System;
using Items_System.Items;
using Player_Controller;
using RayCastSystem;
using ResourceOresSystem;
using UnityEngine;

namespace InHandItems
{
    public class ResourceGatheringObject : DelayItem, IViewable
    {
        private const string GatheringViewPath = "Weapon/View/GatheringObjectView";

        [Header("Attached Components")] [SerializeField]
        private AnimationClip _gatheringAnimation;

        [SerializeField] private InHandItems.InHandAnimations.GatheringObjectAnimator _gatheringObjectAnimator;

        [Header("Main Params")] [SerializeField]
        private Tool _gatheringTool;

        [SerializeField] private LayerMask _rayCastMask;

        [SerializeField] private float _maxGatheringDistance;

        private GatheringObjectView _view;
        private bool _isGathering;

        private Raycaster _rayCaster;

        private void Start()
        {
            _view = Instantiate(Resources.Load<GatheringObjectView>(GatheringViewPath), transform);
            _view.Init(this);
            _rayCaster = new Raycaster();
        }

        private void Update()
        {
            if (!_isGathering) return;
            TryGather();
        }

        public void SetGathering(bool value)
            => _isGathering = value;

        private void TryGather()
            => _gatheringObjectAnimator.Attack();
        
        public void Gather()
        {
            PlayerNetCode.Singleton.PlayerMeleeDamager.TryDamage(_gatheringTool, _gatheringAnimation.length);
            StartCoroutine(RecoverRoutine(_gatheringAnimation.length));
            if (!_rayCaster.TryRaycast<ResourceOre>("Ore", _maxGatheringDistance, out ResourceOre targetResourceOre,
                    _rayCastMask, out RaycastHit hitInfo)) return;

            if (!targetResourceOre.CanUseTool(_gatheringTool)) return;
            targetResourceOre.MinusHp(InventoryHandler.singleton.ActiveItem, out bool destroyed, hitInfo.point,
                hitInfo.normal);
            PlayerNetCode.Singleton.PlayerSoundsPlayer.PlayHit(targetResourceOre.GatheringClip);
        }
    }
}