using Unity.Netcode;

namespace Storage_System
{
    [System.Serializable]
    public struct CustomSendingInventoryDataCell : INetworkSerializable
    {
        public int Id;
        public int Count;
        public int Hp;

        public CustomSendingInventoryDataCell(int id, int count, int hp)
        {
            Id = id;
            Count = count;
            Hp = hp;
        }
        
        public CustomSendingInventoryDataCell(CustomSendingInventoryDataCell data)
        {
            Id = data.Id;
            Count = data.Count;
            Hp = data.Hp;
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Id);
            serializer.SerializeValue(ref Count);
            serializer.SerializeValue(ref Hp);
        }
    }

    [System.Serializable]
    public struct CustomSendingInventoryData : INetworkSerializable
    {
        public CustomSendingInventoryDataCell[] Cells;

        public CustomSendingInventoryData(CustomSendingInventoryDataCell[] cells)
        {
            this.Cells = cells;
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Cells);
        }
    }
}