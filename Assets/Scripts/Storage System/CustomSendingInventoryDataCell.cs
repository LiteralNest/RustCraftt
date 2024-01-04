using Unity.Netcode;

namespace Storage_System
{
    [System.Serializable]
    public struct CustomSendingInventoryDataCell : INetworkSerializable
    {
        public int Id;
        public int Count;
        public int Hp;
        public int Ammo;

        public CustomSendingInventoryDataCell(int id, int count, int hp, int ammo)
        {
            Id = id;
            Count = count;
            Hp = hp;
            Ammo = ammo;
        }

        public CustomSendingInventoryDataCell(CustomSendingInventoryDataCell data)
        {
            Id = data.Id;
            Count = data.Count;
            Hp = data.Hp;
            Ammo = data.Ammo;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Id);
            serializer.SerializeValue(ref Count);
            serializer.SerializeValue(ref Hp);
            serializer.SerializeValue(ref Ammo);
        }
    }
}