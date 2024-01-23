using InHandItems.InHandViewSystem;
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
            var view = Instantiate(Resources.Load<InHandView>(ViewName), this.transform);
            view.Init(this);
        }

        public void TryThrow()
        {
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
            InventoryHandler.singleton.RemoveActiveSlotDisplayer();
            MultiplayObjectsPool.singleton.InstantiateObjectServerRpc(GetComponent<MultiplayInstanceId>().Id,
                _spawnPoint.position, Quaternion.identity, _throwForce, Camera.main.transform.forward);
        }
    }
}