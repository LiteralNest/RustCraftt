using System;
using System.Collections.Generic;
using CloudStorageSystem.CloudStorageServices;
using Unity.Netcode;
using UnityEngine;

namespace CloudStorageSystem.Blocks
{
    public class BuildingStructuresCloudSaver : CloudService
    {
        private List<BuildingStructureSendingData> _blockPositions = new();

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            CloudSaveEventsContainer.OnBuildingBlockSpawned += UpdateBlockList;
            CloudSaveEventsContainer.OnBuildingBlockUpgraded += UpdateBlockLevel;
            CloudSaveEventsContainer.OnBuildingBlockHpChanged += UpdateBlockHp;
            _blockPositions = new List<BuildingStructureSendingData>();
        }

        private void OnDisable()
        {
            if (!IsServer) return;
            CloudSaveEventsContainer.OnBuildingBlockSpawned -= UpdateBlockList;
            CloudSaveEventsContainer.OnBuildingBlockUpgraded -= UpdateBlockLevel;
            CloudSaveEventsContainer.OnBuildingBlockHpChanged -= UpdateBlockHp;
        }

        private void UpdateBlockList(int posX, int posY, int posZ)
            => _blockPositions.Add(new BuildingStructureSendingData(posX, posY, posZ, 0, 0));


        private int GetBlockIndexByPosition(Vector3 position)
        {
            var fixedPos = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
            for (int i = 0; i < _blockPositions.Count; i++)
            {
                if (_blockPositions[i].X == fixedPos.x && _blockPositions[i].Y == fixedPos.y &&
                    _blockPositions[i].Z == fixedPos.z)
                    return i;
            }

            throw new Exception("Can't find block with position: " + position);
        }

        private void UpdateBlockLevel(Vector3 position, int level)
        {
            var index = GetBlockIndexByPosition(position);
            var cachedBlockData = _blockPositions[index];
            _blockPositions[index] = new BuildingStructureSendingData(cachedBlockData.X, cachedBlockData.Y,
                cachedBlockData.Z, cachedBlockData.Hp, level);
        }
        
        private void UpdateBlockHp(Vector3 position, int hp)
        {
            var index = GetBlockIndexByPosition(position);
            var cachedBlockData = _blockPositions[index];
            _blockPositions[index] = new BuildingStructureSendingData(cachedBlockData.X, cachedBlockData.Y,
                cachedBlockData.Z, hp, cachedBlockData.Level);
        }

        public override void SaveData()
        {
            var data = new SendingBlocksData();
            data.BlockPositions = _blockPositions;

            ServerDataHandler dataHandler = new();
            StartCoroutine(dataHandler.SendDataCoroutine("Blocks", data));
        }
    }
}