using System.Collections;
using System.Collections.Generic;
using ResourceOresSystem;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

namespace TerrainTools
{
    public class OreObjectsPlacer : NetworkBehaviour
    {
        [SerializeField] private string _objectParh = "NetObject/";
        [SerializeField] private Vector2Int _size = new Vector2Int(100, 100);
        [SerializeField] private Vector2Int _offset = new Vector2Int(3, 3);
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private int _spawningPrefabsCount = 10;
        [SerializeField] private int _maxAttemptCount = 100;
        [SerializeField] private float _regeneratingTime = 2;
        [SerializeField] private List<string> _tags;
        [SerializeField] private List<Ore> _prefabs = new List<Ore>();

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 fixedPosition =
                new Vector3(transform.position.x + _size.x / 2, 0, transform.position.z + _size.y / 2);
            Gizmos.DrawWireCube(fixedPosition, new Vector3(_size.x, 1000, _size.y));
        }
#endif

        private void Start()
        {
            if (!IsServer) return;
            foreach (var prefab in _prefabs)
                prefab.Init(this);
        }

        public IEnumerator RegenerateObjectRoutine(Ore destroyedObject)
        {
            RemoveObject(destroyedObject);
            yield return new WaitForSeconds(_regeneratingTime);
            RandomlySpawnPrefabs(1);
        }

        [Button]
        [ContextMenu("Regenerate")]
        public void Regenerate()
        {
            if (!IsServer) return;
            ClearPrefabs();
            RandomlySpawnPrefabs(_spawningPrefabsCount);
        }

        private void RemoveObject(Ore destroyedObject)
        {
            _prefabs.Remove(destroyedObject);
            destroyedObject.GetComponent<NetworkObject>().Despawn();
            Destroy(destroyedObject.gameObject);
        }

        private void ClearPrefabs()
        {
            foreach (var prefab in _prefabs)
                Destroy(prefab.gameObject);
            _prefabs.Clear();
        }

        private bool ThereIsYCoordinate(float x, float z, out int y)
        {
            y = default;
            var worldPosition = new Vector3(x, 10000, z);
            var ray = new Ray(worldPosition, Vector3.down);
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, _layerMask)) return false;
            if (!_tags.Contains(hit.collider.tag)) return false;
            y = (int)hit.point.y;
            return true;
        }

        private bool ConflictsWithOther(Vector3Int position)
        {
            foreach (var prefab in _prefabs)
            {
                int prefabX = Mathf.RoundToInt(prefab.transform.position.x);
                int prefabZ = Mathf.RoundToInt(prefab.transform.position.z);

                if (prefabX >= position.x - _offset.x && prefabX <= position.x + _offset.x) return true;
                if (prefabZ >= position.z - _offset.y && prefabZ <= position.z + _offset.y) return true;
            }

            return false;
        }

        private bool PrefabExists(Vector3Int position)
        {
            foreach (var prefab in _prefabs)
            {
                int prefabX = Mathf.RoundToInt(prefab.transform.position.x);
                int prefabZ = Mathf.RoundToInt(prefab.transform.position.z);

                if (prefabX == position.x && prefabZ == position.z) return true;
            }

            return false;
        }

        private Vector3Int GetRandomlyGeneratedCoords()
        {
            int x = 0;
            int z = 0;
            while (true)
            {
                x = Random.Range(_offset.x, _size.x - _offset.x + 1) + (int)transform.position.x;
                z = Random.Range(_offset.y, _size.y - _offset.y + 1) + (int)transform.position.z;
                if (PrefabExists(new Vector3Int(x, 0, z))) continue;
                break;
            }

            return new Vector3Int(x, 0, z);
        }

        private void RandomlySpawnPrefabs(int count)
        {
            int spawnedPrefabsCount = 0;
            int attemptCount = 0;
            while (spawnedPrefabsCount < count)
            {
                if (attemptCount >= _maxAttemptCount) break;
                attemptCount++;
                Vector3Int position = GetRandomlyGeneratedCoords();

                if (ConflictsWithOther(position)) continue;
                if (!ThereIsYCoordinate(position.x, position.z, out int y)) continue;

                position.y = y;
                SpawnObject(position);
                attemptCount = 0;
                spawnedPrefabsCount++;
            }
        }

        private void SpawnObject(Vector3 position)
        {
            var prefab = Resources.Load<Ore>(_objectParh);

            var instance = Instantiate(prefab, position,
                Quaternion.identity, transform);
            instance.GetComponent<NetworkObject>().Spawn();
            instance.Init(this);
            _prefabs.Add(instance);
        }
    }
}