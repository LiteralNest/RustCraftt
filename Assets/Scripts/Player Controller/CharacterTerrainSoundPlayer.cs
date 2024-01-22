using System.Collections;
using ProceduralGeneration.Scripts.SavingSystem;
using ScriptableObjects;
using Sound_System;
using UnityEngine;

namespace Player_Controller
{
    public class CharacterTerrainSoundPlayer : MonoBehaviour
    {
        [SerializeField] private PlayerSoundsPlayer _playerSoundsPlayer;
        [SerializeField] private BlockDataBase _blocksData;
        [SerializeField] private LayerMask _terrainLayer;
        [SerializeField] private PlayerController _playerController;
        
        private bool _canPlaySound;
        private Vector2Int _previousPlayerWorldPosition;

        private void Start()
            => _canPlaySound = true;

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

        public void ChangePosition()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1000, _terrainLayer))
            {
                var blocksHandler = hit.collider.GetComponent<ChunkBlocksHanlder>();
                if (blocksHandler == null) return;
                var pos = gameObject.transform.position;
                var block = blocksHandler.ChunkSavingData.GetBlock(new Vector2Int((int)pos.x, (int)pos.z));
                var info = _blocksData.GetInfo(block.T);
                if (info != null)
                {
                    StartCoroutine(PlaySound(info.AudioClip));
                }
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