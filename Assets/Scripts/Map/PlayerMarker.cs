using System;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
    [SerializeField] private Vector2 _mapSizeUnit;
    [SerializeField] private Vector2 _uiMapSizePx;

    [SerializeField] private RectTransform _playerMarker;
    [SerializeField] private Transform _player;
    
    private Vector2 _spaceConversionFactor;

    private void Awake()
    {
        _spaceConversionFactor = new Vector2(_uiMapSizePx.x / _mapSizeUnit.x, _uiMapSizePx.y / _mapSizeUnit.y);
    
        
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player has the correct tag.");
        }
        
        var playerPosition = new Vector2(_player.position.x, _player.position.y);
        var playerUIPosition = Vector3.Scale(playerPosition, _spaceConversionFactor);
        _playerMarker.anchoredPosition = playerUIPosition;
    }

    private void Update()
    {
        
    }
}