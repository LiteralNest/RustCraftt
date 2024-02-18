using Unity.Netcode;

namespace Cloud.CloudStorageSystem.CloudStorageServices
{
    public abstract class CloudService: NetworkBehaviour
    {
        public abstract void SaveData();
    }
}