using Sirenix.OdinInspector;
using UnityEngine;

namespace Test
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundsTester : MonoBehaviour
    {
        private AudioSource _audioSource;

        [SerializeField] private AudioClip _testClip;

        private void Start()
            => _audioSource = GetComponent<AudioSource>();
        
        [Button]
        public void PlayTestClip()
        {
            _audioSource.PlayOneShot(_testClip);
        }
    }
}