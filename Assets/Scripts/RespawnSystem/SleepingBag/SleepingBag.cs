using System.Collections;
using Building_System.Placing_Objects;
using Events;
using Map;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Web.User;

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

        private int _cachedReloadTime;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _cachedReloadTime = _reloadTime.Value;
            _reloadTime.Value = 0;
        }

        public void Init(int ownerId)
            => SetPlayerIdServerRpc(ownerId);


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

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerIdServerRpc(int id)
        {
            if (!IsServer) return;
            _playerId.Value = id;
            CheckPlayerIdClientRpc();
        }

        private IEnumerator ReloadRoutine()
        {
            while (_reloadTime.Value > 0)
            {
                yield return new WaitForSeconds(1);
                _reloadTime.Value--;
            }
        }

        [ClientRpc]
        private void CheckPlayerIdClientRpc()
        {
            if (_mapPoint == null) return;
            bool value = _playerId.Value == UserDataHandler.singleton.UserData.Id;
            _mapPoint.SetActive(value);
            _targetCanvas.transform.eulerAngles = new Vector3(90, 0, 90);
            GlobalEventsContainer.SleepingBagSpawned?.Invoke(this);
            _targetCanvas.worldCamera = MapCamera.Singleton.TargetCamera;
        }
    }
}