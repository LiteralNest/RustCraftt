using UnityEngine;

namespace Sound_System.TerrainSounds
{
    public class TerrainSoundInteractor : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _stepClips;

        public AudioClip[] StepClips => _stepClips;
    }
}