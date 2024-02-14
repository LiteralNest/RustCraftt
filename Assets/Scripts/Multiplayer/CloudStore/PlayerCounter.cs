using CloudStorageSystem.CloudStorageServices;
using UniRx;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Multiplayer.CloudStore
{
    public class PlayerCounter : NetworkBehaviour
    {
        private string _serverIp;
        private ServerDataHandler _dataHandler = new ServerDataHandler();

        private ReactiveProperty<int> _currentPlayersCount = new ReactiveProperty<int>();

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _currentPlayersCount.Subscribe(RegisterNewUserServerRpc);

            if (IsServer)
                _serverIp = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address;

            if (IsClient)
                _currentPlayersCount.Value++;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (IsServer) return;
            _currentPlayersCount.Value--;
        }

        [ServerRpc(RequireOwnership = false)]
        private void RegisterNewUserServerRpc(int value)
        {
            if (!IsServer) return;
            var data = new ServerData();
            data.ServerIp = _serverIp;
            data.ServerName = "Server";
            data.PlayersCount = value;
            _dataHandler.SendDataAsync(data.ServerName, data);
        }
    }
}