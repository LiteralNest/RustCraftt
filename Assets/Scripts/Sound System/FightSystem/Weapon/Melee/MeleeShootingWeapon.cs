using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using Items_System.Items.Weapon;
using Player_Controller;
using UnityEngine;
using UnityEngine.Serialization;


namespace FightSystem.Weapon.Melee
{
    public class MeleeShootingWeapon : DelayItem, IViewable
    {
        private const string ViewName = "Weapon/View/MeleeShootingWeaponView";

        [Header("Main Params")] [SerializeField]
        private AnimationClip _attackAnimation;

        [SerializeField] private MeleeWeapon _targetWeapon;

        [Header("Attached Components")] [SerializeField]
        private Transform _viewSpawningPoint;

        [FormerlySerializedAs("inHandAnimator")] [Header("ThrowingObject")] [SerializeField]
        private ThrowingInHandAnimator _inHandAnimator;

        [SerializeField] private WeaponThrower _weaponThrower;

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
        }

        private void Update()
        {
            if (!_isAttacking || IsRecovering) return;
            PlayerNetCode.Singleton.PlayerMeleeDamager.TryDamage(_targetWeapon.Damage, 1);
            StartCoroutine(RecoverRoutine(_attackAnimation.length));
        }

        public void SetScope()
            => _inHandAnimator.SetScope();

        public void SetThrow()
        {
            _inHandAnimator.SetThrow();
            _weaponThrower.ThrowSpear();
        }

        public void SetAttack(bool value)
        {
            _isAttacking = value;
            _inHandAnimator.HandleAttack(value);
        }
    }
}