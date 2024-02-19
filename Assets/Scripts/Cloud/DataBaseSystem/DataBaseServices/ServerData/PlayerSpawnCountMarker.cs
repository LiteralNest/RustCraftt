using Unity.Netcode;

namespace Cloud.DataBaseSystem.DataBaseServices.ServerData
{
    public class PlayerSpawnCountMarker : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsServer) return;
            PlayersCounter.Singleton.CurrentPlayersCount.Value++;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if(!IsServer) return;
            PlayersCounter.Singleton.CurrentPlayersCount.Value--;
        }
    }
}