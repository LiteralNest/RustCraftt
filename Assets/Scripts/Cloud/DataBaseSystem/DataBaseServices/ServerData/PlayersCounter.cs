using UniRx;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Cloud.DataBaseSystem.DataBaseServices.ServerData
{
    public class PlayersCounter : NetworkBehaviour
    {
        public static PlayersCounter Singleton { get; private set; }

        private string _serverIp;
        private ReactiveProperty<int> _currentPlayersCount = new ReactiveProperty<int>();
        public ReactiveProperty<int> CurrentPlayersCount { get => _currentPlayersCount; set => _currentPlayersCount = value; } 

        private void Awake()
            => Singleton = this;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsServer) return;
            _serverIp = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address;
            _currentPlayersCount.Subscribe(RegisterNewUserOnServer);
            RegisterNewUserOnServer(_currentPlayersCount.Value);
        }

        private void RegisterNewUserOnServer(int value)
        {
            if (!IsServer) return;
            ServerDataBaseHandler handler = new ServerDataBaseHandler();
            handler.UpdateServerDataAsync(_serverIp, value.ToString());
        }
    }
}