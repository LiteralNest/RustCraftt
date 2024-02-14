using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace CloudStorageSystem.CloudStorageServices
{
    public class CloudServicesContainer : MonoBehaviour
    {
        [Tooltip("Seconds")] [Range(1, 3600)] [SerializeField]
        private float _timeBetweenSaves = 1f;

        [SerializeField] private List<CloudService> _cloudServices;

        private void Start()
            => StartCoroutine(SaveDataCoroutine());

        private IEnumerator SaveDataCoroutine()
        {
            yield return new WaitForSeconds(_timeBetweenSaves);
            foreach (var cloudService in _cloudServices)
                cloudService.SaveData();
            GlobalEventsContainer.OnChatMessageCreated?.Invoke("[Server] Data Saved");
            StartCoroutine(SaveDataCoroutine());
        }
    }
}