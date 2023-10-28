using System.Collections;
using System.Collections.Generic;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.Serialization;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int _mapWidth = 10;
    [SerializeField] private int _mapHeight = 10;
    [SerializeField] private float _blockSize = 1f;

    public int MapWidth => _mapWidth;
    public int MapHeight => _mapHeight;
    public float BlockSize => _blockSize;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_mapWidth * _blockSize, 1, _mapHeight * _blockSize));
    }
}