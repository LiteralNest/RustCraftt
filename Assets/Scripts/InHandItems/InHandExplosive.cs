using InHandItems.InHandAnimations.Weapon;
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

        [SerializeField] protected float _throwForce = 10f;
        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] protected ExplosiveAnimator _explosiveAnimator;

        private void Start()
        {
            var view = Instantiate(Resources.Load<InHandView>(ViewName), this.transform);
            view.Init(this);
        }

        public void TryThrow()
            => _explosiveAnimator.SetThrow();

        public virtual void SpawnPrefab()
        {
            MultiplayObjectsPool.singleton.InstantiateObjectServerRpc(GetComponent<MultiplayInstanceId>().Id,
                _spawnPoint.position, Quaternion.identity, _throwForce, Camera.main.transform.forward);
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
            InventoryHandler.singleton.RemoveActiveSlotDisplayer();
        }
    }
}