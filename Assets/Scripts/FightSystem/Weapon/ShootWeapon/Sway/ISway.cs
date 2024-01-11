namespace FightSystem.Weapon.ShootWeapon.Sway
{
    public interface ISway
    {
        public void UpdateSway();
        public bool CanSway { get; set; }
    }
}