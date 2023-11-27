using System.Collections;
using Character_Stats;
using UnityEngine;

namespace Items_System.GunTrap
{
    public class GunTrap : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GunTrapAmmo _ammo;

        [Header("Main Stats")]
        [SerializeField] private float _damageAmount = 5f;
        [SerializeField] private float _rayDistance = 5f;
        [SerializeField] private LayerMask _playerMask;
        [SerializeField] private float _fireReload = 1f;

        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _shotClip;
        
        [Header("Effect")]
        [SerializeField] private float _effectsDisplayingTime = 1f;
        [SerializeField] private GameObject _vfxEffect;
        [SerializeField] private GameObject _lightEffect;
        
        private Ray _ray;
        private bool _canShoot = true;

        private void Start()
            => _ray = new Ray(transform.position, transform.forward);

        private void Update()
            => TryShoot();

        private IEnumerator ReloadRoutine()
        {
            _canShoot = false;
            yield return new WaitForSeconds(_fireReload);
            _canShoot = true;
        }

        private IEnumerator ShowEffectsRoutine()
        {
            _vfxEffect.SetActive(true);
            _lightEffect.SetActive(true);
            yield return new WaitForSeconds(_effectsDisplayingTime);
            _vfxEffect.SetActive(false);
            _lightEffect.SetActive(false);
        }

        private void TryShoot()
        {
            if (!_canShoot || !_ammo.CanShot()) return;
            if (!Physics.Raycast(_ray, out var hit, _rayDistance, _playerMask)) return;
            if (!hit.collider.CompareTag("Player")) return;
            var hpHandler = hit.collider.GetComponent<CharacterHpHandler>();
            if(!hpHandler) return;
            hpHandler.GetDamage((int)_damageAmount);
            _audioSource.PlayOneShot(_shotClip);
            _ammo.RemoveAmmo();
            StartCoroutine(ShowEffectsRoutine());
            StartCoroutine(ReloadRoutine());
        }
    }
}