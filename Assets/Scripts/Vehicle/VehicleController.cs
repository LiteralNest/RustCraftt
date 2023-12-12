using System.Linq;
using System.Threading.Tasks;
using Building_System.NetWorking;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace Vehicle
{
    public class VehicleController : NetworkBehaviour
    {
        [SerializeField] protected Vector3 _offset = new Vector3(0f, 3f, 0f);
        protected PlayerNetCode _currentPlayer;

        [ClientRpc]
        private void AssignCurrentPlayerClientRpc(ulong playerId)
        {
            var players = FindObjectsOfType<PlayerNetCode>().ToList();
            foreach (var player in players)
            {
                if (player.NetworkObjectId != playerId) continue;
                _currentPlayer = player;
            }
        }

        private void SetPlayer(ulong networkId)
        {
            if (!IsServer) return;
            var players = FindObjectsOfType<PlayerNetCode>().ToList();
            foreach (var player in players)
            {
                var networkObject = player.GetComponent<NetworkObject>();
                if (networkObject.NetworkObjectId != networkId) continue;
                networkObject.TrySetParent(this.transform);
                GetComponent<NetworkObject>().ChangeOwnership(networkObject.OwnerClientId);
                GetComponent<NetworkObject>().DontDestroyWithOwner = true;
                player.ChangePositionClientRpc(transform.position + _offset);
                AssignCurrentPlayerClientRpc(networkId);
                return;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        protected void SitServerRpc(ulong playerId)
        {
            if (!IsServer) return;
            SetPlayer(playerId);
            _currentPlayer.FreezeControllerClientRpc();
        }
        
        
        [ClientRpc]
        private void ResetPlayerClientRpc()
            => _currentPlayer = null;
        
        private void ResetPlayer()
        {
            if (_currentPlayer == null) return;
            _currentPlayer.GetComponent<NetworkObject>().TryRemoveParent();
            ResetPlayerClientRpc();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void StandUpServerRpc(ulong playerId)
        {
            if (!IsServer) return;
            _currentPlayer.UnFreezeControllerClientRpc();
            ResetPlayer();
        }
    }
}