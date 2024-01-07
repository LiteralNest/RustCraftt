using Unity.Netcode;

namespace TechTree
{
    [System.Serializable]
    public struct CustomSendingTechnologyData : INetworkSerializable
    {
        public int[] ItemsId;
        public int UserId;

        public CustomSendingTechnologyData(int[] itemsId, int userId)
        {
            ItemsId = itemsId;
            UserId = userId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ItemsId);
            serializer.SerializeValue(ref UserId);
        }

        public void AddItem(int itemId)
        {
            var buff = new int[ItemsId.Length + 1];
            ItemsId.CopyTo(buff, 0);
            buff[ItemsId.Length] = itemId;
            ItemsId = buff;
        }
    }
}