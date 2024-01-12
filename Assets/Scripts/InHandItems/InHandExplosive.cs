using InHandItems.InHandViewSystem;
using Items_System.Items;
using Multiplayer.Multiplay_Instances;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace InHandItems
{
    public class InHandExplosive : NetworkBehaviour, IViewable
    {
        private const string ViewName = "Weapon/View/ExplosiveView";
        
        [SerializeField] private float _throwForce = 10f;
        [SerializeField] private Transform _spawnPoint;

        private void Start()
        {
            var view = Instantiate(Resources.Load<InHandView>(ViewName));
            view.Init(this);
        } 
        
        public void TryThrow()
        {
            SpawnAndThrowExplosiveServerRpc(_spawnPoint.position, GetComponent<MultiplayInstanceId>().Id);
            PlayerNetCode.Singleton.InHandObjectsContainer.SetDefaultHands();
            InventoryHandler.singleton.RemoveActiveSlotDisplayer();
        } 
    
        [ServerRpc(RequireOwnership = false)]
        private void SpawnAndThrowExplosiveServerRpc(Vector3 position, int id)
        {
            if(!IsServer) return;
            var obj = MultiplayObjectsPool.singleton.GetMultiplayInstanceIdById(id).gameObject;
            var explosive = Instantiate(obj, position, Quaternion.identity);
            explosive.GetComponent<NetworkObject>().Spawn(true);
            var rb = explosive.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(Camera.main.transform.forward * _throwForce, ForceMode.Impulse);
        }
    }
}