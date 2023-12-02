using Unity.Netcode;

namespace Multiplayer.CustomData
{
    [System.Serializable]
    public struct AuthorizedUsersData : INetworkSerializable
    {
        public int[] AuthorizedIds;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref AuthorizedIds);
        }
        
        public AuthorizedUsersData(int[] AuthorizedIds)
        {
            this.AuthorizedIds = AuthorizedIds;
        }

    }
}
