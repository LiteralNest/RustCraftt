using System.Collections.Generic;
using System.IO;
using Chunk;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProceduralGeneration.Scripts.Mesh
{
    [System.Serializable]
    public class BlockData
    {
        public Vector3Int Position;
        public BlockType Type;
    }

    public class BlockPositionSaver : MonoBehaviour
    {
        [SerializeField] private GameObject _terrain;

        private string _savePath = "Assets/GeneratedMeshes/";

        [ContextMenu("Save")]
        public void SaveTopBlockPositions()
        {
            List<BlockData> blockDataList = new List<BlockData>();

            foreach (var chunkRenderer in _terrain.GetComponentsInChildren<ChunkRenderer>())
            {
                var chunkData = chunkRenderer.ChunkData;

                for (int x = 0; x < ChunkRenderer.ChunkWidth; x++)
                {
                    for (int z = 0; z < ChunkRenderer.ChunkWidth; z++)
                    {
                        Vector3Int blockPosition = GetTopBlockPositionInChunk(chunkData, x, z);
                        if (blockPosition != Vector3Int.zero)
                        {
                            BlockData blockData = new BlockData
                            {
                                Position = blockPosition,
                                Type = chunkData.Blocks[x + z * ChunkRenderer.ChunkWidth]
                            };
                            blockDataList.Add(blockData);
                        }
                    }
                }
            }

            string jsonData = JsonUtility.ToJson(blockDataList.ToArray(), true);
            _savePath += gameObject.name + ".json";
            File.WriteAllText(_savePath, jsonData);
        }

        [ContextMenu("Load")]
        public void LoadTopBlockPositions()
        {
            _savePath += gameObject.name + ".json";
            if (File.Exists(_savePath))
            {
                string jsonData = File.ReadAllText(_savePath);
                BlockData[] blockDataArray = JsonUtility.FromJson<BlockData[]>(jsonData);
                
                // Используйте данные, как вам нужно
            }
            else
            {
                Debug.LogWarning("Save file not found.");
            }
        }

        private Vector3Int GetTopBlockPositionInChunk(ChunkData chunkData, int x, int z)
        {
            for (int y = ChunkRenderer.ChunkHeight - 1; y >= 0; y--)
            {
                int index = x + y * ChunkRenderer.ChunkWidthSq + z * ChunkRenderer.ChunkWidth;
                if (chunkData.Blocks[index] != BlockType.Air)
                {
                    return new Vector3Int(x + chunkData.ChunkPosition.x * ChunkRenderer.ChunkWidth, y,
                        z + chunkData.ChunkPosition.y * ChunkRenderer.ChunkWidth);
                }
            }

            return Vector3Int.zero;
        }
    }
}
