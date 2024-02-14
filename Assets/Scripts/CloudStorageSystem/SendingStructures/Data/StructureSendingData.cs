using CustomMathSystem;
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
            X = CustomMath.GetParsedFloatToInt(position.x);
            Y = CustomMath.GetParsedFloatToInt(position.y);
            Z = CustomMath.GetParsedFloatToInt(position.z);
            RotX = CustomMath.GetParsedFloatToInt(rotation.x);
            RotY = CustomMath.GetParsedFloatToInt(rotation.y);
            RotZ = CustomMath.GetParsedFloatToInt(rotation.z);
            Inventory = new CustomSendingInventoryData(inventory.Cells);
        }
    }
}