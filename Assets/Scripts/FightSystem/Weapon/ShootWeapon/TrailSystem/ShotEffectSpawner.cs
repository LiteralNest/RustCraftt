using System.Collections;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

namespace FightSystem.Weapon.ShootWeapon.TrailSystem
{
    public class ShotEffectSpawner : NetworkBehaviour
    {
        [Header("Trail")] 
        [
            SerializeField]
        private Transform _fpSpawningPos;

        [SerializeField] private Transform _tpSpawningPos;
        [SerializeField] private TrailRenderer _bulletTrail;

        [Header("VFX")] [SerializeField] private VisualEffect _shotVfxFp;
        [SerializeField] private VisualEffect _shotVfxTp;
        [SerializeField] private float _flameEffectDuration = 1;
        
        private IEnumerator PlayVfx(VisualEffect effect)
        {
            effect.Play();
            yield return new WaitForSeconds(_flameEffectDuration);
            effect.Stop();
        }

        [ClientRpc]
        private void DisplayEffectClientRpc()
        {
            StartCoroutine(PlayVfx(_shotVfxFp));
            StartCoroutine(PlayVfx(_shotVfxTp));
        }

        [ServerRpc(RequireOwnership = false)]
        public void DisplayEffectServerRpc()
        {
            if (!IsServer) return;
            DisplayEffectClientRpc();
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

        [ClientRpc]
        private void SpawnTrailClientRpc(ulong ownerId, int bulletSpeed, Vector3 hitPoint)
        {
            if (PlayerNetCode.Singleton.OwnerClientId == ownerId)
                StartCoroutine(SpawnTrailRoutine(hitPoint, _fpSpawningPos.position, bulletSpeed));
            else
                StartCoroutine(SpawnTrailRoutine(hitPoint, _tpSpawningPos.position, bulletSpeed));
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnTrailServerRpc(ulong ownerId, int bulletSpeed, Vector3 hitPoint)
        {
            if (!IsServer) return;
            SpawnTrailClientRpc(ownerId, bulletSpeed, hitPoint);
        }
    }
}