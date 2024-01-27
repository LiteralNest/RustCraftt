using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Sound_System
{
    public class PlayerSoundsPlayer : NetworkBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<SoundSLot> _soundSlots = new List<SoundSLot>();
        
        [ServerRpc(RequireOwnership = false)]
        private void PlayOneShotServerRpc(int soundId)
        {
            if (!IsServer) return;
            PlayOneShotClientRpc(soundId);
        }

        [ClientRpc]
        private void PlayOneShotClientRpc(int soundId)
            => PlaySoundLocal(soundId);

        public void PlayHit(AudioClip clip)
        {
            foreach (var slot in _soundSlots)
            {
                if (slot.Clip != clip) continue;
                PlayOneShotServerRpc(slot.Id);
            }
        }

        public void PlaySoundLocal(AudioClip clip)
        {
            foreach (var slot in _soundSlots)
            {
                if (slot.Clip != clip) continue;
                PlaySoundLocal(slot.Id);
            }
        }

        private void PlaySoundLocal(int soundId)
        {
            foreach (var slot in _soundSlots)
            {
                if (slot.Id != soundId) continue;
                _audioSource.volume = slot.Volume;
                _audioSource.PlayOneShot(slot.Clip);
            }
        }
    }
}