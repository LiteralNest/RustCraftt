using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon.Sway
{
    public abstract class Sway : MonoBehaviour, ISway
    {
        protected Transform SwayTransform;
        public bool CanSway { get; set; }
        
        public abstract void UpdateSway();

        public virtual void Init(Transform swayTransform)
            => SwayTransform = swayTransform;
    }
}