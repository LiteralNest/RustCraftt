using UnityEngine;

public class MapSizeGetter : MonoBehaviour
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