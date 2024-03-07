using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using Items_System.Items.WeaponSystem;
using Player_Controller;
using UnityEngine;
using UnityEngine.Serialization;

namespace FightSystem.Weapon.Melee
{
    public class MeleeShootingWeapon : MonoBehaviour, IViewable
    {
        private const string ViewName = "Weapon/View/MeleeShootingWeaponView";

        [Header("Main Params")] [SerializeField]
        private MeleeWeapon _targetWeapon;

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

        public void SetScope()
            => _inHandAnimator.SetScope();

        public void Damage()
            => PlayerNetCode.Singleton.PlayerMeleeDamager.TryDamage(_targetWeapon);

        public void SetThrow()
            => _inHandAnimator.SetThrow();

        public void SetAttack(bool value)
            => _inHandAnimator.HandleAttack(value);
    }
}