using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Sound_System
{
    public class NetworkSoundPlayer : NetworkBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<SoundSLot> _soundSlots = new List<SoundSLot>();

        private int GetIdByClip(AudioClip clip)
        {
            for (int i = 0; i < _soundSlots.Count; i++)
            {
                if (_soundSlots[i].Clip == clip) return i;
            }

            return 0;
        }

        public void PlayOneShot(AudioClip clip)
            => PlaySoundClientRpc(true, GetIdByClip(clip), GetComponent<NetworkObject>().NetworkObjectId);

        public void PlayOneShotFromClient(AudioClip clip)
            => PlaySoundServerRpc(true, GetIdByClip(clip));

        public void PlayClip(AudioClip clip)
            => PlaySoundServerRpc(false, GetIdByClip(clip));

        [ServerRpc(RequireOwnership = false)]
        private void PlaySoundServerRpc(bool isOneShot, int id)
        {
            if (!IsServer) return;
            PlaySoundClientRpc(isOneShot, id, GetComponent<NetworkObject>().NetworkObjectId);
        }

        [ClientRpc]
        private void PlaySoundClientRpc(bool isOneShot, int clipId, ulong networkObjectId)
        {
            if (GetComponent<NetworkObject>().NetworkObjectId != networkObjectId) return;
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