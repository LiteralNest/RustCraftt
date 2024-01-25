using FightSystem.Weapon.ShootWeapon.Ammo;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
    public class AmmoObjectsPool : NetworkBehaviour
    {
        public static AmmoObjectsPool Singleton { get; private set; }
        
        public Arrow _arrowPrefab;

        private void Awake()
        {
            Singleton = this;
        }
        
        [ServerRpc]
        public void SpawnArrowServerRpc(Vector3 position, Quaternion rotation, Vector3 force)
        {
            var arrow = Instantiate(_arrowPrefab, position, rotation);
            arrow.GetComponent<NetworkObject>().Spawn();
            arrow.ArrowFly(force);
        }
    }
}