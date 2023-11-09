namespace Fight_System.Weapon.ShootWeapon
{
    public interface IWeapon
    {
        void Attack(bool value);
        void Reload();
        bool CanReload();
    }
}