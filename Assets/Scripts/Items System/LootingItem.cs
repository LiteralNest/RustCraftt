using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class LootingItem : NetworkBehaviour
{
    public Item Item => _item;
    [SerializeField] private Item _item;
    public int Count { get; set; }

    private void Start()
        => gameObject.tag = "LootingItem";

    [ServerRpc(RequireOwnership = false)]
    public void PickUpServerRpc()
    {
        if (IsServer)
        {
           GetComponent<NetworkObject>().Despawn();
        }
        Destroy(gameObject);
    }
}