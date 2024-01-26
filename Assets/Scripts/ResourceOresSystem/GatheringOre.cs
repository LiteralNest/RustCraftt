using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ResourceOresSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public class GatheringOre : Ore
    {
        [SerializeField] private bool _shouldDelete;
        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private List<Collider> _colliders;
        [SerializeField] private NetworkVariable<bool> _recovering = new(false);
        [SerializeField] private float _recoveringTime;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _recovering.OnValueChanged += ((value, newValue) => DisplayRenderers(newValue));
        }

        private void Start()
        {
            gameObject.tag = "Gathering";
        }

        private void DisplayRenderers(bool value)
        {
            foreach (var renderer in _renderers)
                renderer.enabled = value;
            foreach (var collider in _colliders)
                collider.enabled = value;
        }

        public void Gather()
        {
            if (Recovering) return;
            AddResourcesToInventory();
            MinusHpServerRpc();
            if (_shouldDelete)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RecoverServerRpc()
        {
            if(!IsServer) return;
            Recovering = true;
            StartCoroutine(StartRecovering());
        }

        private IEnumerator StartRecovering()
        {
            _recovering.Value = true;
            yield return new WaitForSeconds(_recoveringTime);
            _recovering.Value = false;
        }
    }
}