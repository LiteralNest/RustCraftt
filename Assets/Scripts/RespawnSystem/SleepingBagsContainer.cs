using System.Collections.Generic;
using Events;
using UnityEngine;

namespace RespawnSystem
{
    public class SleepingBagsContainer : MonoBehaviour
    {
        [SerializeField] private List<SleepingBag.SleepingBag> _sleepingBags = new List<SleepingBag.SleepingBag>(); 

        private void OnEnable()
        {
            GlobalEventsContainer.SleepingBagSpawned += OnSleepingBagSpawned;
        }

        private void OnDisable()
        {
            GlobalEventsContainer.SleepingBagSpawned -= OnSleepingBagSpawned;
        }

        private void OnSleepingBagSpawned(SleepingBag.SleepingBag sleepingBag)
        {
            if (_sleepingBags.Contains(sleepingBag)) return;
            _sleepingBags.Add(sleepingBag);
        }
    }
}