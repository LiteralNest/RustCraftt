using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Sound_System
{
    public class PlayerSoundsPlayer : NetworkBehaviour
    {
        public static PlayerSoundsPlayer Singleton { get; private set; }
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<SoundSLot> _soundSlots = new List<SoundSLot>();
        
        private async void Start()
        {
            await Task.Delay(1000);
            if(!IsOwner) return;
            Singleton = this;
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void PlayOneShotServerRpc(int soundId, float volume)
        {
            if (!IsServer) return;
            PlayOneShotClientRpc(soundId, volume);
        }

        [ClientRpc]
        private void PlayOneShotClientRpc(int soundId, float volume)
        {
            _audioSource.volume = volume;
            _audioSource.PlayOneShot(_soundSlots[soundId].Clip);
        }

        public void PlayHit(AudioClip clip)
        {
            foreach (var slot in _soundSlots)
            {
                if(slot.Clip != clip) continue;
                PlayOneShotServerRpc(slot.Id, slot.Volume);
            }
        }
    }
}