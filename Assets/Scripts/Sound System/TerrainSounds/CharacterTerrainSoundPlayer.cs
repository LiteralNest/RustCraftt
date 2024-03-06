using System;
using Player_Controller;
using Sound_System;
using Sound_System.TerrainSounds;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterTerrainSoundPlayer : NetworkBehaviour
{
    [SerializeField] private PlayerSoundsPlayer _playerSoundsPlayer;
    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private LayerMask _waterLayer;
    [SerializeField] private float _rayCastDistance = 0.5f;
    private AudioClip[] _terrainStepClips;

    private int _cachedClipIndex = -1;
    private bool _isInWater;
    
    private void SetTerrainStepClips(AudioClip[] terrainStepClips)
    {
        _terrainStepClips = terrainStepClips;
    }
    
    public void ChangePosition()
    {
        if(!IsOwner) return;

        if (_isInWater)
        {
            SetTerrainStepClips(null);
            return;
        }

        if (!_isInWater && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _rayCastDistance, _terrainLayer))
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            _isInWater = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            _isInWater = false;
    }
}