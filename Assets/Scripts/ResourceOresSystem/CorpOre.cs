using System.Collections;
using System.Collections.Generic;
using Cloud.CloudStorageSystem;
using InteractSystem;
using Items_System.Items.Abstract;
using Player_Controller;
using PlayerDeathSystem;
using UnityEngine;

namespace ResourceOresSystem
{
    public class CorpOre : ResourceOre, IRayCastHpDisplayer, IRaycastInteractable
    {
        [SerializeField] private Sprite _displayIcon;
        [SerializeField] private List<GameObject> _displayingObjects = new();
        [SerializeField] private List<GameObject> _activatingObjects = new();
        [SerializeField] private BackPack _backPack;

        protected override IEnumerator DestroyRoutine()
        {
            foreach (var obj in _displayingObjects)
                obj.SetActive(false);
            foreach (var obj in _activatingObjects)
                obj.SetActive(true);
            DoAfterDestroy();
            yield return base.DestroyRoutine();
        }

        public void DisplayData()
        {
            if (PlayerNetCode.Singleton == null) return;
            PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayHp(CachedMaxHp, _currentHp.Value);
        }

        protected override void DoAfterDamage()
            => CloudSaveEventsContainer.OnBackPackHpChanged?.Invoke(_backPack.BackPackId, _currentHp.Value);

        protected override void DoAfterDestroy()
            => CloudSaveEventsContainer.OnBackPackDestroyed?.Invoke(_backPack.BackPackId);

        public string GetDisplayText()
            => "Gather";

        public bool CanDisplayInteract()
            => true;
        
        public void Interact()
        {
        }

        public Sprite GetIcon()
            => _displayIcon;

        public bool CanInteract()
            => _currentHp.Value > 0;
    }
}