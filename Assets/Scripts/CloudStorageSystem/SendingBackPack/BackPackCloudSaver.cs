using System;
using System.Collections.Generic;
using CloudStorageSystem.CloudStorageServices;
using CloudStorageSystem.SendingBackPack.Data;
using Storage_System;
using UnityEngine;

namespace CloudStorageSystem.SendingBackPack
{
    public class BackPackCloudSaver : CloudService
    {
        private List<BackPackSlotData> _savingData = new List<BackPackSlotData>();

        private void OnEnable()
        {
            CloudSaveEventsContainer.OnBackPackSpawned += AddBackPack;
            CloudSaveEventsContainer.OnBackPackHpChanged += AssignBackPackHp;
            CloudSaveEventsContainer.OnBackPackInventoryChanged += AssignBackPackInventory;
            CloudSaveEventsContainer.OnBackPackDestroyed += RemoveBackPack;
        }

        private void OnDisable()
        {
            CloudSaveEventsContainer.OnBackPackSpawned -= AddBackPack;
            CloudSaveEventsContainer.OnBackPackHpChanged -= AssignBackPackHp;
            CloudSaveEventsContainer.OnBackPackInventoryChanged -= AssignBackPackInventory;
            CloudSaveEventsContainer.OnBackPackDestroyed -= RemoveBackPack;
        }

        public override void SaveData()
        {
            ServerDataHandler dataHandler = new();
            BackPackListData sendingData = new(_savingData);
            dataHandler.SendDataAsync(CloudStorageKeys.BackPacks, sendingData);
        }

        private int GetBackpackIndexByPosition(int backPackId)
        {
            for (int i = 0; i < _savingData.Count; i++)
            {
                if(_savingData[i].BackPackId == backPackId)
                    return i;
            }
            throw new Exception("BackPack with id" + backPackId + " not found");
        }

        private void AddBackPack(int backPackId, Vector3 position, CustomSendingInventoryData itemsNetData,
            string nickName,
            int ownerId, bool wasDisconnected, float hp)
            => _savingData.Add(new BackPackSlotData(backPackId, position, itemsNetData, nickName, ownerId, wasDisconnected, hp));

        private void RemoveBackPack(int backPackId)
            => _savingData.RemoveAt(GetBackpackIndexByPosition(backPackId));

        private void AssignBackPackHp(int backPackId, float hp)
        {
            int index = GetBackpackIndexByPosition(backPackId);
            var cachedSlot = _savingData[index];
            cachedSlot.Hp = hp;
            _savingData[index] = cachedSlot;
        }

        private void AssignBackPackInventory(int backPackId, CustomSendingInventoryData itemsNetData)
        {
            int index = GetBackpackIndexByPosition(backPackId);
            var cachedSlot = _savingData[index];
            cachedSlot.ItemsNetData = itemsNetData;
            _savingData[index] = cachedSlot;
        }
    }
}