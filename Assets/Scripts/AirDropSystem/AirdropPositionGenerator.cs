using System.Collections.Generic;
using UnityEngine;

namespace AirDropSystem
{
    public class AirdropPositionGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _airdropPrefab;
        [SerializeField] private MapSizeGetter _mapSizeGetter;
        [SerializeField] private float _airdropHeight = 80f;
        [SerializeField] private float _spawnZoneOffsetY;

        private Vector3 _point1;
        private Vector3 _point2;
        private Vector3 _spawnPoint;
        private float _randomDistance;

        private void SpawnAirdrop(Vector3 spawnPoint)
        {
            Instantiate(_airdropPrefab, spawnPoint, Quaternion.identity);
        }

        private void CalculateAndSpawn()
        {
            var randomEdge1 = Random.Range(0, 4);
            _point1 = GenerateRandomEdgePoint(randomEdge1);

            var randomEdge2 = GetRandomEdgeDifferentFrom(randomEdge1);
            _point2 = GenerateRandomEdgePoint(randomEdge2);

            _randomDistance = Random.Range(0f, Vector3.Distance(_point1, _point2));

            var spawnPoint = CalculateRandomSpawnPoint(_point1, _point2, _randomDistance);
            SpawnAirdrop(spawnPoint);
        }

        #region CalculationOfPosition

        private int GetRandomEdgeDifferentFrom(int edgeToAvoid)
        {
            var availableEdges = new List<int> { 0, 1, 2, 3 };
            availableEdges.Remove(edgeToAvoid);
            var randomIndex = Random.Range(0, availableEdges.Count);
            return availableEdges[randomIndex];
        }

        private Vector3 GenerateRandomEdgePoint(int randomEdge)
        {
            var availableEdges = new List<int> { 0, 1, 2, 3 };
            availableEdges.Remove(randomEdge);
            var randomEdgeIndex = Random.Range(0, availableEdges.Count);
            var randomEdge2 = availableEdges[randomEdgeIndex];

            var edgePoint = Vector3.zero;

            var halfWidth = _mapSizeGetter.MapWidth * _mapSizeGetter.BlockSize / 2;
            var halfHeight = _mapSizeGetter.MapHeight * _mapSizeGetter.BlockSize / 2;
            var center = _mapSizeGetter.transform.position;

            switch (randomEdge)
            {
                case 0:
                    edgePoint = center + new Vector3(Random.Range(-halfWidth, halfWidth), _airdropHeight, halfHeight);
                    break;
                case 1:
                    edgePoint = center + new Vector3(halfWidth, _airdropHeight, Random.Range(-halfHeight, halfHeight));
                    break;
                case 2:
                    edgePoint = center + new Vector3(Random.Range(-halfWidth, halfWidth), _airdropHeight, -halfHeight);
                    break;
                case 3:
                    edgePoint = center + new Vector3(-halfWidth, _airdropHeight, Random.Range(-halfHeight, halfHeight));
                    break;
            }

            return edgePoint;
        }

        private Vector3 CalculateRandomSpawnPoint(Vector3 p1, Vector3 p2, float t)
        {
            t = Random.Range(0f, 1f);
            var spawnPoint = Vector3.Lerp(p1, p2, t);
            spawnPoint.y = _airdropHeight;

            spawnPoint = new Vector3(spawnPoint.x, _spawnZoneOffsetY, spawnPoint.z);
            _spawnPoint = spawnPoint;

            return spawnPoint;
        }

        #endregion

#if UNITY_EDITOR

        #region UnityEditorTest

        private void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 400, 160), "Generate Airdrop"))
            {
                CalculateAndSpawn();
            }
        }

        private void OnDrawGizmos()
        {
            var halfWidth = _mapSizeGetter.MapWidth * _mapSizeGetter.BlockSize / 2;
            var halfHeight = _mapSizeGetter.MapHeight * _mapSizeGetter.BlockSize / 2;
            var center = _mapSizeGetter.transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center + new Vector3(0, _spawnZoneOffsetY, 0),
                new Vector3(halfWidth * 2, _airdropHeight, halfHeight * 2));

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_point1, _point2);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_point1, 10f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_point2, 10f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_spawnPoint, 10f);
        }

        #endregion
#endif
    }
}
