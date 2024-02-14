using Unity.Netcode;

namespace CloudStorageSystem.CloudStorageServices
{
    public abstract class CloudService : NetworkBehaviour
    {
        public abstract void SaveData();
    }
}