using Building_System.Upgrading;
using Unity.Netcode;
using UnityEngine;

public class PlacingObject : BuildingStructure, IHammerInteractable
{
    [field: SerializeField] public Item TargetItem { get; private set; }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjectServerRpc()
    {
        if(!IsServer) return;
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
    
    #region IHammerInteractable

    public bool CanBePickUp()
        => true;

    public void PickUp()
    {
        InventorySlotsContainer.singleton.AddItemToDesiredSlot(TargetItem, 1);
        DestroyObjectServerRpc();
    }
    
    public bool CanBeUpgraded()
        => false;

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public bool CanBeRepaired()
        => false;

    public void Repair()
    {
        throw new System.NotImplementedException();
    }

    public bool CanBeDestroyed()
        => false;

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }
    
    #endregion
}