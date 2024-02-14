using UnityEngine;
using Storage_System;

namespace CloudStorageSystem.SendingStructures
{
    public struct StructureSendingData
    {
        public int StructureId;
        public int Hp;
        public int X;
        public int Y;
        public int Z;
        public int RotX;
        public int RotY;
        public int RotZ;
        public CustomSendingInventoryData Inventory;

        public StructureSendingData(int structureId, int hp, Vector3 position, Vector3 rotation,
            CustomSendingInventoryData inventory)
        {
            StructureId = structureId;
            Hp = hp;
            X = (int)position.x;
            Y = (int)position.y;
            Z = (int)position.z;
            RotX = (int)rotation.x;
            RotY = (int)rotation.y;
            RotZ = (int)rotation.z;
            Inventory = new CustomSendingInventoryData(inventory.Cells);
        }
    }
}