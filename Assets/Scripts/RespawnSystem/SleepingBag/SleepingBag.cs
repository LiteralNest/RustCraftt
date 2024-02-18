using System.Collections;
using Building_System.Building.Placing_Objects;
using Cloud.DataBaseSystem.UserData;
using Events;
using Map;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace RespawnSystem.SleepingBag
{
    public class SleepingBag : NetworkBehaviour, IPlacingObjectInteractable
    {
        [Header("Network Parameters")] [SerializeField]
        private NetworkVariable<FixedString64Bytes> _name = new NetworkVariable<FixedString64Bytes>("Untitled");

        public NetworkVariable<FixedString64Bytes> Name => _name;
        [SerializeField] private NetworkVariable<int> _reloadTime = new NetworkVariable<int>(300);
        public NetworkVariable<int> ReloadTime => _reloadTime;
        private NetworkVariable<int> _playerId = new NetworkVariable<int>(0);

        [Header("Main Params")] [SerializeField]
        private Canvas _targetCanvas;

        [SerializeField] private GameObject _mapPoint;
        [SerializeField] private Transform _respawnPoint;
        public Transform RespawnPoint => _respawnPoint;

        private int _cachedReloadTime;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _cachedReloadTime = _reloadTime.Value;
            _reloadTime.Value = 0;

            CheckPlayerId(_playerId.Value);

            _playerId.OnValueChanged += (int oldValue, int newValue) => { CheckPlayerId(newValue); };
        }

        private void Start()
        {
            if (!GetComponent<NetworkObject>().IsSpawned) return;
            CheckPlayerId(_playerId.Value);
        }

        public void Init(int ownerId)
            => SetPlayerId(ownerId);


        [ServerRpc(RequireOwnership = false)]
        public void RespawnPlayerServerRpc()
        {
            if (!IsServer) return;
            _reloadTime.Value = _cachedReloadTime;
            StartCoroutine(ReloadRoutine());
        }

        [ServerRpc(RequireOwnership = false)]
        public void RenameServerRpc(string value)
        {
            if (!IsServer) return;
            _name.Value = value;
        }

        private void SetPlayerId(int id)
        {
            if (!IsServer) return;
            _playerId.Value = id;
        }

        private IEnumerator ReloadRoutine()
        {
            while (_reloadTime.Value > 0)
            {
                yield return new WaitForSeconds(1);
                _reloadTime.Value--;
            }
        }

        private void CheckPlayerId(int newValue)
        {
            if (_mapPoint == null) return;
            bool value = newValue == UserDataHandler.Singleton.UserData.Id;
            _mapPoint.SetActive(value);
            _targetCanvas.transform.eulerAngles = new Vector3(90, 0, 90);
            GlobalEventsContainer.SleepingBagSpawned?.Invoke(this);
            _targetCanvas.worldCamera = MapCamera.Singleton.TargetCamera;
        }
    }
}