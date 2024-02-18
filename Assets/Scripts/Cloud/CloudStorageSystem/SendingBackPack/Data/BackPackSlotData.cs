using CustomMathSystem;
using Storage_System;
using UnityEngine;

namespace Cloud.CloudStorageSystem.SendingBackPack.Data
{
    public struct BackPackSlotData
    {
        public CustomSendingInventoryData ItemsNetData { get; set; }
        public string NickName { get; set; }
        public int OwnerId { get; set; }
        public bool WasDisconnected { get; set; }
        public float Hp { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int BackPackId { get; set; }

        public BackPackSlotData(int backPackId, Vector3 position, CustomSendingInventoryData itemsNetData, string nickName, int ownerId,
            bool wasDisconnected, float hp)
        {
            BackPackId = backPackId;
            X = CustomMath.GetParsedFloatToInt(position.x);
            Y = CustomMath.GetParsedFloatToInt(position.y);
            Z = CustomMath.GetParsedFloatToInt(position.z);
            ItemsNetData = itemsNetData;
            NickName = nickName;
            OwnerId = ownerId;
            WasDisconnected = wasDisconnected;
            Hp = hp;
        }
    }
}