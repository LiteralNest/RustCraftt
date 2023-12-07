using Unity.Netcode;
using UnityEngine;

namespace Sound_System
{
    public class PlayerSoundsPlayer : NetworkBehaviour
    {
        public static PlayerSoundsPlayer Singleton { get; private set; }
        
        [Header("Attached Components")] [SerializeField]
        private AudioSource _audioSource;

        [Header("Audio Clips")] [SerializeField]
        private AudioClip _hitClip;
        [SerializeField] private AudioClip _headShotClip;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsOwner) return;
            Singleton = this;
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void PlayOneShotServerRpc(int soundId)
        {
            if (!IsServer) return;
            PlayOneShotClientRpc(soundId);
        }

        [ClientRpc]
        private void PlayOneShotClientRpc(int soundId)
        {
            if(soundId == 1)
                _audioSource.PlayOneShot(_hitClip);
            else if(soundId == 2)
                _audioSource.PlayOneShot(_headShotClip);
        }
        
        public void PlayHitSound()
            => PlayOneShotServerRpc(1);
        
        public void PlayHeadShotSound()
            => PlayOneShotServerRpc(2);
    }
}