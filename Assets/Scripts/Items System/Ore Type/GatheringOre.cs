using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class GatheringOre : Ore
{
    private void Start()
    {
        base.Start();
        gameObject.tag = "Gathering";
    }

    public void Gather()
    {
        if(Recovering) return;
        InventoryHandler.singleton.InventorySlotsContainer.AddItemToDesiredSlot(_targetResource, Random.Range(_addingCount.x, _addingCount.y + 1));
        MinusHpServerRpc();
    }
}