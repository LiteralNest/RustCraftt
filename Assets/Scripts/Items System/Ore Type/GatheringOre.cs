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
        AddResourcesToInventory();
        MinusHpServerRpc();
    }
}