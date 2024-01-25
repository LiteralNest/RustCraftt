using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using Items_System.Items.Weapon;
using Player_Controller;
using UnityEngine;


namespace FightSystem.Weapon.Melee
{
    public class MeleeShootingWeapon : DelayItem, IViewable
    {
        private const string ViewName = "Weapon/View/MeleeShootingWeaponView";

        [Header("Main Params")] 
        [SerializeField] private AnimationClip _attackAnimation;
        [SerializeField] private MeleeWeapon _targetWeapon;

        [Header("Attached Components")] [SerializeField]
        private Transform _viewSpawningPoint;

        [Header("ThrowingObject")] [SerializeField]
        private ThrowingInHandAnimator inHandAnimator;

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
            if (!_isAttacking || IsRecovering) return;
            PlayerNetCode.Singleton.PlayerMeleeDamager.TryDamage(_targetWeapon.Damage, 1);
            StartCoroutine(RecoverRoutine(_attackAnimation.length));
            inHandAnimator.Attack();
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