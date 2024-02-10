using Unity.Netcode;
using UnityEngine;

namespace Sound_System.TerrainSounds
{
    public class CharacterTerrainSoundPlayer : NetworkBehaviour
    {
        [SerializeField] private PlayerSoundsPlayer _playerSoundsPlayer;
        [SerializeField] private LayerMask _terrainLayer;
        [SerializeField] private float _rayCastDistance = 2f;
        private AudioClip[] _terrainStepClips;

        private int _cachedClipIndex = -1;
        
        private void SetTerrainStepClips(AudioClip[] terrainStepClips)
        {
            _terrainStepClips = terrainStepClips;
        }

        public void ChangePosition()
        {
          if(!IsOwner) return;
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _rayCastDistance, _terrainLayer))
            {
                if (!hit.collider.TryGetComponent(out TerrainSoundInteractor soundInteractor)) return;
                SetTerrainStepClips(soundInteractor.StepClips);
                while (true)
                {
                    var rand = Random.Range(0, _terrainStepClips.Length);
                    if (rand == _cachedClipIndex) continue;
                    _cachedClipIndex = rand;
                    break;
                }
                var nextStepClip = _terrainStepClips[_cachedClipIndex];
                _playerSoundsPlayer.PlayHit(nextStepClip);
            }
        }
    }
}