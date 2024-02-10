using Sound_System.TerrainSounds;
using UnityEngine;

namespace Sound_System
{
    public class PlayerStepHandler : MonoBehaviour
    {
        [SerializeField] private CharacterTerrainSoundPlayer _characterTerrainSoundPlayer;

        public void HandleStep()
            => _characterTerrainSoundPlayer.ChangePosition();
    }
}