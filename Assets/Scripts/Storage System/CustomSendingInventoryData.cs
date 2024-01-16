using Unity.Netcode;

namespace Storage_System
{
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