using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InstantiatingItemsPool : NetworkBehaviour
{
    public static InstantiatingItemsPool sigleton { get; set; }

    [SerializeField] private List<LootingItem> _items = new List<LootingItem>();

    private void Awake()
        => sigleton = this;

    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectServerRpc(int inputItemId, int count, Vector3 position)
    {
        if(!IsServer) return;
        foreach (var ore in _items)
        {
            if (ore.Item.Id == inputItemId)
            {
                var obj = Instantiate(ore, position, Quaternion.identity);
                obj.Count = count;
                obj.NetworkObject.DontDestroyWithOwner = true;
                ore.gameObject.GetComponent<NetworkObject>().Spawn();
                return;
            }
            Debug.LogError("Can't find object with id " + inputItemId);
        }
    }
}