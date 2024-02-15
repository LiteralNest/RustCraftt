using System;
using System.Collections.Generic;
using CloudStorageSystem.CloudStorageServices;
using CloudStorageSystem.SendingStructures.Data;
using CustomMathSystem;
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
            CloudSaveEventsContainer.OnStructureDestroyed += RemoveStructure;
        }

        private void OnDisable()
        {
            CloudSaveEventsContainer.OnStructureSpawned -= AddStructure;
            CloudSaveEventsContainer.OnStructureInventoryChanged -= ApplyStructureInventory;
            CloudSaveEventsContainer.OnStructureHpChanged -= ApplyStructureHp;
            CloudSaveEventsContainer.OnStructureDestroyed -= RemoveStructure;
        }

        public override void SaveData()
        {
            ServerDataHandler dataHandler = new();
            StructuresSendingDataList sendingData = new(_data);
            dataHandler.SendDataAsync(CloudStorageKeys.Structures, sendingData);
        }

        private int GetListStructureIndexByPosition(Vector3 position)
        {
            for (int i = 0; i < _data.Count; i++)
                if ((int)_data[i].X == CustomMath.GetParsedFloatToInt(position.x) &&
                    (int)_data[i].Y == CustomMath.GetParsedFloatToInt(position.y) && (int)_data[i].Z == CustomMath.GetParsedFloatToInt(position.z))
                    return i;
            throw new Exception("Can't find structure at position: " + "X: " + (int)position.x + " Y: " + position.y +
                                " Z: " + (int)position.z);
        }

        private void AddStructure(int structureId, Vector3 position, Vector3 rotation)
        {
            _data.Add(new StructureSendingData(structureId, 0, position, rotation,
                new CustomSendingInventoryData(Array.Empty<CustomSendingInventoryDataCell>())));
        }

        private void RemoveStructure(Vector3 position)
        {
            int id = GetListStructureIndexByPosition(position);
            _data.RemoveAt(id);
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