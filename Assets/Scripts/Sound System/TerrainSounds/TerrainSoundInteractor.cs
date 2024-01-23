using UnityEngine;

namespace Sound_System.TerrainSounds
{
    public class TerrainSoundInteractor : MonoBehaviour
    {
        [SerializeField] private AudioClip _stepClip;

        public AudioClip StepClip => _stepClip;
    }
}