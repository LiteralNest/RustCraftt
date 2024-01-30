using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Sound_System
{
    [RequireComponent(typeof(AudioSource))]
    public class NetworkSoundPlayer : NetworkBehaviour
    {
        [SerializeField] private List<SoundSLot> _soundSlots = new List<SoundSLot>();

        private AudioSource _audioSource;
        
        private void Awake()
            => _audioSource = GetComponent<AudioSource>();

        private int GetIdByClip(AudioClip clip)
        {
            for (int i = 0; i < _soundSlots.Count; i++)
            {
                if (_soundSlots[i].Clip == clip) return i;
            }

            return 0;
        }

        public void PlayOneShot(AudioClip clip)
            => PlaySoundClientRpc(true, GetIdByClip(clip));

        public void PlayOneShotFromClient(AudioClip clip)
            => PlaySoundServerRpc(true, GetIdByClip(clip));

        public void PlayClip(AudioClip clip)
            => PlaySoundServerRpc(false, GetIdByClip(clip));

        [ServerRpc(RequireOwnership = false)]
        private void PlaySoundServerRpc(bool isOneShot, int id)
        {
            if (!IsServer) return;
            PlaySoundClientRpc(isOneShot, id);
        }

        [ClientRpc]
        private void PlaySoundClientRpc(bool isOneShot, int clipId)
        {
            AudioClip targetClip = _soundSlots[clipId].Clip;
            if (isOneShot)
                _audioSource.PlayOneShot(targetClip);
            else
            {
                _audioSource.clip = targetClip;
                _audioSource.Play();
            }
        }
    }
}