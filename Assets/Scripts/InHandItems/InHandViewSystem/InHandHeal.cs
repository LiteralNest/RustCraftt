using System.Collections;
using InHandItems.InHandAnimations;
using Items_System.Items;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace InHandItems.InHandViewSystem
{
    public class InHandHeal : NetworkBehaviour, IViewable
    {
        private const string ViewName = "Weapon/View/HealView";

        [SerializeField] private AnimationClip _healingClip;
        [SerializeField] private Medicine _targetMedicine;
        [SerializeField] private InHandHealAnimator _animator;

        private HealView _view;

        private void OnEnable()
        {
            if (_view)
                _view.DisplayHealButton(true);
        }

        private void Start()
        {
            _view = Instantiate(Resources.Load<HealView>(ViewName), this.transform);
            _view.Init(this);
        }

        public void Heal()
        {
            _view.DisplayHealButton(false);
            StartCoroutine(PlayHealRoutine());
            InventoryHandler.singleton.Stats.PlusStat(CharacterStatType.Health, _targetMedicine.AddingValue);
            var activeSlotDisplay = InventoryHandler.singleton.ActiveSlotDisplayer;
            InventoryHandler.singleton.CharacterInventory.RemoveItemCountFromSlotServerRpc(activeSlotDisplay.Index,
                _targetMedicine.Id, 1);
        }

        private IEnumerator PlayHealRoutine()
        {
            _animator.PlayHeal();
            yield return new WaitForSeconds(_healingClip.length);
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
        }
    }
}