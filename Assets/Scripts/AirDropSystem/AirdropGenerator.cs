using System.Collections.Generic;
using UnityEngine;

public class AirdropGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _airdropPrefab;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private float _airdropHeight = 80f;

    private Vector3 _point1;
    private Vector3 _point2;
    private float _randomDistance = 0f;

    private void SpawnAirdrop(Vector3 spawnPoint)
    {
        Instantiate(_airdropPrefab, spawnPoint, Quaternion.identity);
    }

    private void CalculateAndSpawn()
    {
        _point1 = GenerateRandomEdgePoint();
        _point2 = GenerateRandomEdgePoint();

        _randomDistance = Random.Range(0f, Vector3.Distance(_point1, _point2));

        var spawnPoint = CalculateRandomSpawnPoint(_point1, _point2);
        SpawnAirdrop(spawnPoint);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 30), "Generate Airdrop"))
        {
            CalculateAndSpawn();
        }
    }

    private Vector3 GenerateRandomEdgePoint()
    {
        List<int> availableEdges = new List<int> { 0, 1, 2 };
        int randomEdgeIndex = Random.Range(0, availableEdges.Count);
        int randomEdge = availableEdges[randomEdgeIndex];
        availableEdges.RemoveAt(randomEdgeIndex);

        Vector3 edgePoint = Vector3.zero;

        float halfWidth = _mapGenerator.MapWidth * _mapGenerator.BlockSize / 2;
        float halfHeight = _mapGenerator.MapHeight * _mapGenerator.BlockSize / 2;
        Vector3 center = _mapGenerator.transform.position;
        Debug.Log(center);

        switch (randomEdge)
        {
            case 0: 
                center += new Vector3(Random.Range(0, _mapGenerator.MapWidth) * _mapGenerator.BlockSize, 0, _mapGenerator.MapHeight * _mapGenerator.BlockSize);
                break;
            case 1: 
                center += new Vector3(_mapGenerator.MapWidth * _mapGenerator.BlockSize, 0, Random.Range(0, _mapGenerator.MapHeight) * _mapGenerator.BlockSize);
                break;
            case 2: 
                center += new Vector3(Random.Range(0, _mapGenerator.MapWidth) * _mapGenerator.BlockSize, 0, 0);
                break;
        }

        edgePoint = center + new Vector3(Random.Range(-halfWidth, halfWidth), 0, Random.Range(-halfHeight, halfHeight));

        return edgePoint;
    }




    private Vector3 CalculateRandomSpawnPoint(Vector3 p1, Vector3 p2)
    {
        var t = Random.Range(0f, 1f);
        var spawnPoint = Vector3.Lerp(p1, p2, t);
        spawnPoint.y = _airdropHeight;

        float halfWidth = _mapGenerator.MapWidth * _mapGenerator.BlockSize / 2;
        float halfHeight = _mapGenerator.MapHeight * _mapGenerator.BlockSize / 2;
        Vector3 center = _mapGenerator.transform.position;

        spawnPoint = center + new Vector3(Random.Range(-halfWidth, halfWidth), _airdropHeight, Random.Range(-halfHeight, halfHeight));

        return spawnPoint;
    }

    private void OnDrawGizmos()
    {
        float halfWidth = _mapGenerator.MapWidth * _mapGenerator.BlockSize / 2;
        float halfHeight = _mapGenerator.MapHeight * _mapGenerator.BlockSize / 2;
        Vector3 center = _mapGenerator.transform.position;

        Gizmos.color = Color.green; // 
        Gizmos.DrawWireCube(center, new Vector3(halfWidth * 2, _airdropHeight, halfHeight * 2));

        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_point1, _point2);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_point1, 1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_point2, 1f);
    }
}
