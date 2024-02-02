using System.Collections;
using FightSystem.Damage;
using Sound_System;
using Sound_System.FightSystem.Damage;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.Explosive
{
    public abstract class BaseExplosive : NetworkBehaviour
    {
        [Header("Attached Scripts")]
        [SerializeField] protected NetworkSoundPlayer _explosiveSoundPlayer;
        [SerializeField] protected AudioClip _explosiveClip;
        [SerializeField] private GameObject _model;
        [SerializeField] private GameObject _explosionVfx;

        [Header("Main Params")] [SerializeField]
        protected float _explosionRadius = 5f;

        [SerializeField] protected float _maxDamage = 50f;

        [SerializeField] protected float shakeDuration = 0.5f;
        [SerializeField] protected float shakeMagnitude = 0.2f;

        protected CameraShake _cameraShake;
        protected Collider[] _colliders;
        protected bool _hasExploded = false;

        protected virtual void Start()
        {
            _colliders = new Collider[100];
            //How is better?
            _cameraShake = GetComponent<CameraShake>();
        }

        private void DamageObjects()
        {
            if (_hasExploded) return;
            _hasExploded = true;

            var numColliders = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _colliders);

            for (var i = 0; i < numColliders; i++)
            {
                var damageable = _colliders[i].GetComponent<IDamagable>();
                if (damageable == null) continue;

                var distance = Vector3.Distance(transform.position, _colliders[i].transform.position);
                var damage = Mathf.Lerp(_maxDamage, 0f, distance / _explosionRadius);
                damageable.Shake();
                damageable.GetDamage((int)damage, false);
            }
        }

        private IEnumerator PlaySoundRoutine()
        {
            _explosiveSoundPlayer.PlayOneShot(_explosiveClip);
            yield return new WaitForSeconds(_explosiveClip.length);
        }

        [ClientRpc]
        private void DisplayExplosionClientRpc()
        {
            _explosionVfx.SetActive(true);
            _model.SetActive(false);
        }

        private IEnumerator ExplodeRoutine()
        {
            DisplayExplosionClientRpc();
            DamageObjects();
            yield return StartCoroutine(PlaySoundRoutine());
            if (IsServer)
                GetComponent<NetworkObject>().Despawn();
        }

        [ServerRpc(RequireOwnership = false)]
        protected void ExplodeServerRpc()
        {
            if (!IsServer) return;
            StartCoroutine(ExplodeRoutine());
        }
    }
}