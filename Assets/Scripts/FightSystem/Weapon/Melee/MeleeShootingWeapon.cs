using Events;
using FightSystem.Weapon.WeaponAnimations;
using InHandViewSystem;
using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class MeleeShootingWeapon : MonoBehaviour, IViewable
    {
        private const string ViewName = "Weapon/View/MeleeShootingWeaponView";

        [Header("Attached Components")] [SerializeField]
        private Transform _viewSpawningPoint;

        [Header("ThrowingObject")] [SerializeField]
        private ThrowingWeaponAnimator _weaponAnimator;

        [SerializeField] private WeaponThrower _weaponThrower;
        [SerializeField] private GameObject _mainObj;

        private bool _isAttacking;
        private ThrowingInHandView _inHandView;
        private Vector3 _direction;
        private bool _wasScoped;

        private void Start()
        {
            _inHandView = Instantiate(Resources.Load<ThrowingInHandView>(ViewName), _viewSpawningPoint);
            _inHandView.Init(this);
        }

        private void OnEnable()
        {
            if (_inHandView != null)
            {
                _inHandView.gameObject.SetActive(true);
                _inHandView.DisplayAttackButton(true);
                _inHandView.DisplayScopeButton(true);
            }

            DisplayDefault();
            _wasScoped = false;
        }

        private void Update()
        {
            if (_isAttacking)
                _weaponAnimator.Attack();
        }

        public void SetThrowingPosition(bool value)
        {
            _wasScoped = true;
            _mainObj.SetActive(false);
            _weaponThrower.gameObject.SetActive(value);
            _inHandView.DisplayAttackButton(false);
            if (value) return;
            _inHandView.gameObject.SetActive(false);
            _weaponThrower.ThrowSpear();
            gameObject.SetActive(false);
        }

        public void SetAttack(bool value)
            => _isAttacking = value;
        
        private void DisplayDefault()
        {
            _weaponThrower.gameObject.SetActive(false);
            _mainObj.SetActive(true);
        }
    }
}