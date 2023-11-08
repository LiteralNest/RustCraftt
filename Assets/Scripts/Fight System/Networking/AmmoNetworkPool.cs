using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AmmoNetworkPool : NetworkBehaviour
{
    public static AmmoNetworkPool singleton { get; set; }

    [SerializeField] private List<ShootingWeapon> _weapons = new List<ShootingWeapon>();
    [SerializeField] private List<NetworkObject> _netWorkObjects = new List<NetworkObject>();
    
    private void Awake()
        => singleton = this;

    private ShootingWeapon GetWeaponById(int id)
    {
        foreach(var weapon in _weapons)
            if (weapon.Id == id)
                return weapon;
        return null;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectServerRpc(int bulletId, int weaponId ,Vector3 pos, Quaternion rotation,Vector3 directionSpread)
    {
        if(!IsServer) return;
        var obj = Instantiate(_netWorkObjects[bulletId], pos, rotation);
        obj.DontDestroyWithOwner = true;
        obj.Spawn();
        ShootingWeapon weapon = GetWeaponById(weaponId);
    }
}
