using System.Collections;
using System.Collections.Generic;
using InteractSystem;
using Player_Controller;
using UnityEngine;

namespace ResourceOresSystem
{
    public class CorpOre : ResourceOre, IRayCastHpDisplayer, IRaycastInteractable
    {
        [SerializeField] private Sprite _displayIcon;
        [SerializeField] private List<GameObject> _displayingObjects = new();
        [SerializeField] private List<GameObject> _activatingObjects = new();

        protected override IEnumerator DestroyRoutine()
        {
            foreach (var obj in _displayingObjects)
                obj.SetActive(false);
            foreach (var obj in _activatingObjects)
                obj.SetActive(true);
            yield return base.DestroyRoutine();
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

        public Sprite GetIcon()
            => _displayIcon;

        public bool CanInteract()
            => _currentHp.Value > 0;
    }
}