using System.Collections;
using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using Multiplayer.Multiplay_Instances;
using Player_Controller;
using UnityEngine;

namespace InHandItems
{
    public class GranadeExplosive : InHandExplosive
    {
        private const string ViewName = "Weapon/View/GranadeView";

        [SerializeField] private GranadeAnimator _granadeAnimator;
        

        private void Start()
        {
            var view = Instantiate(Resources.Load<GranadeView>(ViewName), this.transform);
            view.Init(this);
        }

        public void Scope()
            => _granadeAnimator.PlayScope();

        public void Throw()
        {
            _granadeAnimator.PlayThrow();
        }

        public override void SpawnPrefab()
        {
            MultiplayObjectsPool.singleton.InstantiateObjectServerRpc(GetComponent<MultiplayInstanceId>().Id,
                _spawnPoint.position, Quaternion.identity, _throwForce, Camera.main.transform.forward);
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
            InventoryHandler.singleton.RemoveActiveSlotDisplayer();
        }
    }
}