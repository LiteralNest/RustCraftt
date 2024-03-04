using System.Collections;
using UnityEngine;

namespace Environment
{
    public class AmbientZone : MonoBehaviour
    {
        [SerializeField] private AudioClip _ambientSound;
    
        private AudioSource _audioSource;
        private Coroutine _coroutine;
        private void Awake()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_ambientSound == null) return;
            if (other.CompareTag("Player"))
            {
                _audioSource = other.GetComponent<AudioSource>();
                _audioSource.clip = _ambientSound;
                _audioSource.Play();
                _coroutine = StartCoroutine(IncreaseVolumeRoutine());
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if(_ambientSound == null) return;
            if (other.CompareTag("Player"))
            {
                if(_coroutine != null)
                    StopCoroutine(_coroutine);
                StartCoroutine(DecreaseVolumeRoutine());
            }
        }
    
        private IEnumerator IncreaseVolumeRoutine()
        {
            if(_audioSource == null) yield break;
            while (_audioSource.volume < 0.4f)
            {
                if(_audioSource == null) yield break;
                _audioSource.volume += 0.05f;
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator DecreaseVolumeRoutine()
        {
            if(_audioSource == null) yield break;
            while (_audioSource.volume > 0)
            {
                if(_audioSource == null) yield break;
                _audioSource.volume -= 0.1f;
                yield return new WaitForSeconds(0.5f);
            }
            _audioSource.Stop();
        }
    }
}