using UnityEngine;

public class ObjectBluePrint : BluePrint
{
    public override void CheckForAvailable()
    {
        CanBePlaced = true;
    }

    public override void Place()
    {
        BuildingsNetworkingSpawner.singleton.SpawnPrefServerRpc(TargetBuildingStructure.Id, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
