using Unity.Netcode;

namespace TechTree
{
    [System.Serializable]
    public struct CustomSendingTechnologyArrayData : INetworkSerializable
    {
        public CustomSendingTechnologyData[] TechnologyArray;

        public CustomSendingTechnologyArrayData(CustomSendingTechnologyData[] array)
        {
            TechnologyArray = array;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref TechnologyArray);
        }
    }
}