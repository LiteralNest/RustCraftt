using System;
using System.Collections.Generic;
using CloudStorageSystem.CloudStorageServices;
using Storage_System;
using UnityEngine;

namespace CloudStorageSystem.SendingStructures
{
    public class StructuresCloudSaver : CloudService
    {
        private List<StructureSendingData> _data = new List<StructureSendingData>();

        private void OnEnable()
        {
            CloudSaveEventsContainer.OnStructureSpawned += AddStructure;
            CloudSaveEventsContainer.OnStructureInventoryChanged += ApplyStructureInventory;
            CloudSaveEventsContainer.OnStructureHpChanged += ApplyStructureHp;
        }

        private void OnDisable()
        {
            CloudSaveEventsContainer.OnStructureSpawned -= AddStructure;
            CloudSaveEventsContainer.OnStructureInventoryChanged -= ApplyStructureInventory;
            CloudSaveEventsContainer.OnStructureHpChanged -= ApplyStructureHp;
        }

        public override void SaveData()
        {
            ServerDataHandler dataHandler = new();
            StructuresSendingDataList sendingData = new(_data);
            dataHandler.SendDataAsync("Structures", sendingData);
        }

        private int GetListStructureIndexByPosition(Vector3 position)
        {
            for (int i = 0; i < _data.Count; i++)
                if (_data[i].X == (int)position.x && _data[i].Y == (int)position.y && _data[i].Z == (int)position.z)
                    return i;
            throw new Exception("Can't find structure at position: " + position);
        }

        private void AddStructure(int structureId, Vector3 position, Vector3 rotation)
        {
            _data.Add(new StructureSendingData(structureId, 0, position, rotation,
                new CustomSendingInventoryData(Array.Empty<CustomSendingInventoryDataCell>())));
        }

        private void ApplyStructureInventory(Vector3 position, CustomSendingInventoryData inventory)
        {
            int id = GetListStructureIndexByPosition(position);
            var cachedSlot = _data[id];
            _data[id] = new StructureSendingData(cachedSlot.StructureId, cachedSlot.Hp, position,
                new Vector3(cachedSlot.RotX, cachedSlot.RotY, cachedSlot.RotZ), inventory);
        }

        private void ApplyStructureHp(int hp, Vector3 position)
        {
            int id = GetListStructureIndexByPosition(position);
            var cachedSlot = _data[id];
            _data[id] = new StructureSendingData(cachedSlot.StructureId, hp, position,
                new Vector3(cachedSlot.RotX, cachedSlot.RotY, cachedSlot.RotZ), cachedSlot.Inventory);
        }
    }
}