using System.Collections.Generic;
using Events;
using RespawnSystem.SleepingBag;
using UnityEngine;

namespace RespawnSystem
{
    public class SleepingBagsViewPooler : MonoBehaviour
    {
        [SerializeField] private List<SleepingBagView> _sleepingBags = new List<SleepingBagView>();

        private void OnEnable()
        {
            GlobalEventsContainer.SleepingBagSpawned += OnSleepingBagSpawned;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.SleepingBagSpawned -= OnSleepingBagSpawned;
        }

        private bool CanSpawnView(SleepingBag.SleepingBag sleepingBag)
        {
            foreach (var view in _sleepingBags)
            {
                if(!view.gameObject.activeSelf) continue;
                if (view.SleepingBag == sleepingBag)
                    return false;
            }
            return true;            
        }

        private SleepingBagView GetAvailableView()
        {
            foreach (var view in _sleepingBags)
            {
                if(view.gameObject.activeSelf) continue;
                return view;
            }
            return null;
        }
        
        private void OnSleepingBagSpawned(SleepingBag.SleepingBag sleepingBag)
        {
            if(!CanSpawnView(sleepingBag)) return;

            var view = GetAvailableView();
            if (view == null) return;
            view.gameObject.SetActive(true);
            view.Init(sleepingBag);
        }
    }
}