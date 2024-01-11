using System.Collections;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon.TrailSystem
{
    public class TrailSpawner : NetworkBehaviour
    {
        [SerializeField] private Transform _fpSpawningPos;
        [SerializeField] private Transform _tpSpawningPos;

        [Header("Trail Settings")] [SerializeField]
        private TrailRenderer _bulletTrail;

        [ServerRpc(RequireOwnership = false)]
        public void SpawnTrailServerRpc(ulong ownerId, int bulletSpeed, Vector3 hitPoint)
        {
            if (!IsServer) return;
            SpawnTrailClientRpc(ownerId, bulletSpeed, hitPoint);
        }

        [ClientRpc]
        private void SpawnTrailClientRpc(ulong ownerId, int bulletSpeed, Vector3 hitPoint)
        {
            if (PlayerNetCode.Singleton.OwnerClientId == ownerId)
                StartCoroutine(SpawnTrailRoutine(hitPoint, _fpSpawningPos.position, bulletSpeed));
            else
                StartCoroutine(SpawnTrailRoutine(hitPoint, _tpSpawningPos.position, bulletSpeed));
        }

        private IEnumerator SpawnTrailRoutine(Vector3 hitPoint, Vector3 pos, int bulletSpeed)
        {
            var trail = Instantiate(_bulletTrail, pos, Quaternion.identity);

            var distance = Vector3.Distance(trail.transform.position, hitPoint);
            var remainingDistance = distance;

            while (remainingDistance > 0)
            {
                trail.transform.position =
                    Vector3.Lerp(trail.transform.position, hitPoint, 1 - (remainingDistance / distance));

                remainingDistance -= bulletSpeed * Time.deltaTime;

                yield return null;
            }

            trail.transform.position = hitPoint;

            Destroy(trail.gameObject, trail.time);
        }
    }
}