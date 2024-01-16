using Unity.Netcode;

namespace PlayerDeathSystem.ArmorsSystem
{
    [System.Serializable]
    public struct CopesArmorsNetData: INetworkSerializable
    {
        public int[] ArmorIds;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ArmorIds);
        }

        public CopesArmorsNetData(int[] armorIds)
        {
            ArmorIds = armorIds;
        }
        
    }
}