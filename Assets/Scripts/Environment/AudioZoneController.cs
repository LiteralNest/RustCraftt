using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Environment
{
    public class AudioZoneController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;
       
        private void OnTriggerEnter(Collider other)
        {
            // Detect entering a zone and adjust effects accordingly
            if (other.CompareTag("Water"))
            {
                _mixer.SetFloat("ReverbAmount", 0.5f);
                other.GetComponent<AudioSource>().Play();
            }
            // else if (other.CompareTag("HighReverbZone"))
            // {
            //     mixer.SetFloat("ReverbAmount", 1f);
            // }
        }

        private void OnTriggerExit(Collider other)
        {
            _mixer.SetFloat("ReverbAmount", 0f);
            other.GetComponent<AudioSource>().Stop();
        }

    }
}