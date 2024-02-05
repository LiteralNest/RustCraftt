namespace FightSystem.Damage
{
    public interface IBuildingDamagable
    {
        public void GetDamageOnServer(int damageItemId);
        public int GetHp();
        public int GetMaxHp();
    }
}