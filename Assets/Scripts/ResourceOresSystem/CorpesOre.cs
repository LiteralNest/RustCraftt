using System.Collections.Generic;
using InteractSystem;
using Player_Controller;
using UnityEngine;

namespace ResourceOresSystem
{
    public class CorpesOre : ResourceOre, IRayCastHpDisplayer, IRaycastInteractable
    {
        [SerializeField] private List<GameObject> _displayingObjects = new();
        [SerializeField] private List<GameObject> _activatingObjects = new();

        protected override void DoAfterDestroying()
        {
            foreach (var obj in _displayingObjects)
                obj.SetActive(false);
            foreach (var obj in _activatingObjects)
                obj.SetActive(true);
        }

        public void DisplayData()
        {
            if (PlayerNetCode.Singleton == null) return;
            PlayerNetCode.Singleton.ObjectHpDisplayer.DisplayHp(CachedMaxHp, _currentHp.Value);
        }

        public string GetDisplayText()
            => "Gather";

        public void Interact()
        {
        }

        public bool CanInteract()
            => _currentHp.Value > 0;
    }
}