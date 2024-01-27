using UnityEngine;

namespace Sound_System
{
    public class GlobalSoundsContainer : MonoBehaviour
    {
        public static GlobalSoundsContainer Singleton { get; set; }

        [field: SerializeField] public AudioClip HitSound { get; set; }

        private void Awake()
            => Singleton = this;
    }
}