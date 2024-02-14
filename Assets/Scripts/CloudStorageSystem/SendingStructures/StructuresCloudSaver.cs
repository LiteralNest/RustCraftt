using CloudStorageSystem.CloudStorageServices;
using Sirenix.OdinInspector;
using Storage_System;
using UnityEngine;

namespace CloudStorageSystem.SendingStructures
{
    public class StructuresCloudSaver : MonoBehaviour
    {
        [Button]
        public void TestSavingData()
        {
            CustomSendingInventoryDataCell[] cells = new CustomSendingInventoryDataCell[1];
            cells[0] = new CustomSendingInventoryDataCell(1, 2, 100, 10);
            StructureSendingData data = new(1, 2, 3, 90, 0, 0, new CustomSendingInventoryData(cells));
            ServerDataHandler dataHandler = new();
            dataHandler.SendDataAsync("Structures", data);
        }
    }

    public struct StructureSendingData
    {
        public int X;
        public int Y;
        public int Z;
        public float RotX;
        public float RotY;
        public float RotZ;
        public CustomSendingInventoryData Inventory;

        public StructureSendingData(int x, int y, int z, float rotX, float rotY, float rotZ,
            CustomSendingInventoryData inventory)
        {
            X = x;
            Y = y;
            Z = z;
            RotX = rotX;
            RotY = rotY;
            RotZ = rotZ;
            Inventory = new CustomSendingInventoryData(inventory.Cells);
        }
    }
}