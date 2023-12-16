using System.Collections;
using Building_System.Placing_Objects;
using Map;
using Unity.Netcode;
using UnityEngine;
using Web.User;

namespace DeathSystem.SleepBag
{
    public class SleepingBag : NetworkBehaviour
    {
        [SerializeField] private Canvas _targetCanvas;
        [SerializeField] private GameObject _mapPoint;
        [SerializeField]
        private NetworkVariable<int> _playerId = new NetworkVariable<int>(0);

        private void Start()
        {
            StartCoroutine(CheckPlayerIdRoutine());
        }
        
        private IEnumerator CheckPlayerIdRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            if(IsServer)
                _playerId.Value = GetComponent<PlacingObject>().OwnerId.Value;
            CheckPlayerId();
        }

        private void CheckPlayerId()
        {
            if(_mapPoint == null) return;
            bool value = _playerId.Value == UserDataHandler.singleton.UserData.Id;
            _mapPoint.SetActive(value);
            _targetCanvas.worldCamera = MapCamera.Singleton.TargetCamera;
        }
    }
}