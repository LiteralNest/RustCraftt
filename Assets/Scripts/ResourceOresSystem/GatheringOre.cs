using System.Collections.Generic;
using System.Collections;
using InteractSystem;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace ResourceOresSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public class GatheringOre : Ore, IRaycastInteractable
    {
        [SerializeField] private Sprite _displayIcon;

        [Header("Attached Scripts")] 
        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private AudioClip _gatherClip;
        [SerializeField] private List<Collider> _colliders;
        [SerializeField] private float _recoveringTime;

        private NetworkVariable<bool> _recovering = new(false);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _recovering.OnValueChanged += ((value, newValue) => DisplayRenderers(!newValue));
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
            if (_recovering.Value) return;
            PlayerNetCode.Singleton.PlayerSoundsPlayer.PlayHit(_gatherClip);
            AddResourcesToInventory();
            RecoverServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RecoverServerRpc()
        {
            if (!IsServer) return;
            StartCoroutine(StartRecovering());
        }

        private IEnumerator StartRecovering()
        {
            _recovering.Value = true;
            yield return new WaitForSeconds(_recoveringTime);
            _recovering.Value = false;
        }

        #region IRaycastInteractable

        public string GetDisplayText()
            => "Gather";

        public void Interact()
            => Gather();

        public Sprite GetIcon()
            => _displayIcon;

        public bool CanInteract()
            => true;

        #endregion
    }
}