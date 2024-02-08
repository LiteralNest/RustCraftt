using System.Collections;
using Player_Controller;
using UnityEngine;

namespace Sound_System.TerrainSounds
{
    public class CharacterTerrainSoundPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerSoundsPlayer _playerSoundsPlayer;
        [SerializeField] private LayerMask _terrainLayer;
        [SerializeField] private PlayerController _playerController;

        private bool _canPlaySound;
        private Vector2Int _previousPlayerWorldPosition;
        private int _currentStepIndex = 0;
        private AudioClip[] _terrainStepClips;
        
        public void SetTerrainStepClips(AudioClip[] terrainStepClips)
        {
            _terrainStepClips = terrainStepClips;
        }
        
        private void Start()
        {
            _canPlaySound = true;
            _currentStepIndex = 0;
        }

        private void Update()
        {
            if (_playerController.IsCrouching) return;
            var playerWorldPosition = Vector3Int.FloorToInt(transform.position);
            var fixedPlayerWorldPosition = new Vector2Int(playerWorldPosition.x, playerWorldPosition.z);

            if (fixedPlayerWorldPosition != _previousPlayerWorldPosition)
            {
                _previousPlayerWorldPosition = fixedPlayerWorldPosition;
                ChangePosition();
            }
        }

        private void ChangePosition()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1000, _terrainLayer))
            {
                if (!hit.collider.TryGetComponent(out TerrainSoundInteractor soundInteractor)) return;
                SetTerrainStepClips(soundInteractor.StepClips);
                var nextStepClip = _terrainStepClips[_currentStepIndex];
                StartCoroutine(PlaySound(nextStepClip));

                _currentStepIndex = (_currentStepIndex + 1) % _terrainStepClips.Length;
            }
        }

        private IEnumerator PlaySound(AudioClip clip)
        {
            if (!_canPlaySound) yield break;
            _playerSoundsPlayer.PlayHit(clip);
            _canPlaySound = false;
            yield return new WaitForSeconds(clip.length);
            _canPlaySound = true;
        }
    }
}