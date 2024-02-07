using FightSystem.Weapon.ShootWeapon.Ammo;
using Items_System.Items.Abstract;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.NetWorking
{
    public class AmmoObjectsPool : NetworkBehaviour
    {
        public static AmmoObjectsPool Singleton { get; private set; }

        [SerializeField] private Arrow _bowArrow;
        [SerializeField] private Arrow _crossBowArrow;
        [SerializeField] private Item _bow;
        [SerializeField] private Item _crossBow;

        private void Awake()
        {
            Singleton = this;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnArrowServerRpc(int weaponId, Vector3 position, Quaternion rotation, Vector3 force)
        {
            if (!IsServer) return;
            Arrow arrow = null;
            if (weaponId == _bow.Id)
                arrow = Instantiate(_bowArrow, position, rotation);
            else
                arrow = Instantiate(_crossBowArrow, position, rotation);
            arrow.GetComponent<NetworkObject>().Spawn();
            arrow.ArrowFly(force);
        }
    }
}