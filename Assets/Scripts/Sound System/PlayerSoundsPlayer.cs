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
            for (int i = 0; i < _soundSlots.Count; i++)
            {
                if (_soundSlots[i].Clip != clip) continue;
                PlayOneShotServerRpc(i);
                return;
            }
        }

        public void PlaySoundLocal(AudioClip clip)
        {
            for (int i = 0; i < _soundSlots.Count; i++)
            {
                if (_soundSlots[i].Clip != clip) continue;
                PlaySoundLocal(i);
                return;
            }
        }

        private void PlaySoundLocal(int soundId)
        {
            var slot = _soundSlots[soundId];
            _audioSource.volume = slot.Volume;
            _audioSource.PlayOneShot(slot.Clip);
        }
    }
}