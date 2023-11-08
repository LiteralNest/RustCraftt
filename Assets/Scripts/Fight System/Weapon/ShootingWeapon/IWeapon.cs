namespace Fight_System.Weapon.ShootingWeapon
{
    public interface IWeapon
    {
        void Attack(bool value);
        void Reload();
        bool CanReload();
    }
}