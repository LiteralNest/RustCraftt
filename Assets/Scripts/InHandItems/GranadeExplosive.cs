using System.Collections;
using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using Multiplayer.Multiplay_Instances;
using Player_Controller;
using UnityEngine;

namespace InHandItems
{
    public class GranadeExplosive : InHandExplosive, IViewable
    {
        private const string ViewName = "Weapon/View/GranadeView";

        [SerializeField] private GranadeAnimator _granadeAnimator;

        private bool _wasThrow;

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
            StartCoroutine(ThrowRoutine());
        }

        private IEnumerator ThrowRoutine()
        {
            if (_wasThrow) yield break;
            MultiplayObjectsPool.singleton.InstantiateObjectServerRpc(GetComponent<MultiplayInstanceId>().Id,
                _spawnPoint.position, Quaternion.identity, _throwForce, Camera.main.transform.forward);
            _wasThrow = true;
            yield return new WaitForSeconds(_throwingClip.length);
            _wasThrow = false;
            PlayerNetCode.Singleton.SetDefaultHandsServerRpc();
            InventoryHandler.singleton.RemoveActiveSlotDisplayer();
        }
    }
}