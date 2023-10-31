using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InstantiatingItemsPool : NetworkBehaviour
{
    public static InstantiatingItemsPool sigleton { get; set; }

    [SerializeField] private List<LootingItem> _items = new List<LootingItem>();

    [SerializeField] private LootingItem _universalDropableItem;

    private void Awake()
        => sigleton = this;

    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectServerRpc(int inputItemId, int count, Vector3 position)
    {
        if(!IsServer) return;
        foreach (var ore in _items)
        {
            if (ore.ItemId.Value == inputItemId)
            {
                var obj = Instantiate(ore, position, Quaternion.identity);
                obj.Count.Value = count;
                obj.NetworkObject.DontDestroyWithOwner = true;
                obj.NetworkObject.Spawn();
                return;
            }
            Debug.LogError("Can't find object with id " + inputItemId);
        }
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    public void SpawnDropableObjectServerRpc(int inputItemId, int count, Vector3 position)
    {
        if(!IsServer) return;
        var obj = Instantiate(_universalDropableItem, position, Quaternion.identity);
        obj.ItemId.Value = inputItemId;
        obj.Count.Value = count;
        obj.NetworkObject.DontDestroyWithOwner = true;
        obj.NetworkObject.Spawn();
    }
}