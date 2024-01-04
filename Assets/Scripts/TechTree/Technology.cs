using System.Collections.Generic;
using Items_System.Items.Abstract;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Technology : MonoBehaviour
{
    [Header("Attached Components")] [SerializeField]
    private Item _scrap;

    [SerializeField] private TechnologyUI _technologyUI;
    [field:SerializeField] public int Cost { get; private set; }

    [SerializeField] private List<Technology> _unlockingTech = new List<Technology>();
    [field: SerializeField] public Item Item { get; private set; }
    [field:SerializeField] public bool _isActive;
    [field: SerializeField] public bool IsResearched { get; private set; }

    private void Awake()
        => _technologyUI.DisplayTech(this);

    private void UnlockTechs()
    {
        foreach (var tech in _unlockingTech)
            tech._isActive = true;
    }

    public bool CanResearch()
    {
        if (!_isActive) return false;
        return InventoryHandler.singleton.CharacterInventory.GetItemCount(_scrap.Id) >= Cost;
    }

    public void Research()
    {
        if (IsResearched || !CanResearch()) return;
        InventoryHandler.singleton.CharacterInventory.RemoveItem(_scrap.Id, Cost);
        IsResearched = true;
        _technologyUI.UnlockTech();
        UnlockTechs();
    }
}


    [System.Serializable]
    public struct CustomSendingTechnologyData : INetworkSerializable
    {
        public int TechId;
        public ulong UserId;

        public CustomSendingTechnologyData(int techId, ulong userId)
        {
            TechId = techId;
            UserId = userId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref TechId);
            serializer.SerializeValue(ref UserId);
        }
    }

    public class TechnologyManager : NetworkBehaviour
    {
        // A network variable to store the technology id and user id
        [field: SerializeField]
        public NetworkVariable<CustomSendingTechnologyData> TechNetData { get; private set; } = new();

        // A reference to the technology prefab
        [SerializeField] private Technology _technologyPrefab;

        // A dictionary to store the technology instances by id
        private Dictionary<int, Technology> _technologies;

        private void Awake()
        {
            _technologies = new Dictionary<int, Technology>();
        }

        
        // A method to check if a technology is available for research by user id
        public bool IsTechnologyAvailableServerRpc(int techId, ulong userId)
        {
            if (_technologies.TryGetValue(techId, out var tech))
            {
                if (tech._isActive && !tech.IsResearched)
                {
                    if (userId == tech.GetComponent<NetworkObject>().OwnerClientId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // A method to spawn the technologies on the server and assign them ids
        [ServerRpc(RequireOwnership = false)]
        public void SpawnTechnologiesServerRpc(CustomSendingTechnologyData data)
        {
            if(!IsServer) return;
            TechNetData.Value = data;
        }


        // A method to research a technology by user id
        [ServerRpc(RequireOwnership = false)]
        public void ResearchTechnologyServerRpc(int techId, ulong userId)
        {
            if (IsTechnologyAvailableServerRpc(techId, userId))
            {
                var tech = _technologies[techId];

                tech.Research();

                TechNetData.Value = new CustomSendingTechnologyData(techId, userId);
            }
        }
    }

