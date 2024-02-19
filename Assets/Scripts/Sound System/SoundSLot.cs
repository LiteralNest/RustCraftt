using UnityEngine;

namespace Sound_System
{
    [System.Serializable]
    public class SoundSLot
    {
        [Range(0f, 1f)]
        public float Volume = 1f;
        public AudioClip Clip;
    }
}
