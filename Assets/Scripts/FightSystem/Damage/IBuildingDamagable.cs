namespace FightSystem.Damage
{
    public interface IBuildingDamagable
    {
        public void GetDamageOnServer(int damageItemId);
        public void GetDamageByExplosive(int explosiveId, float distance, float radius);
        public int GetHp();
        public int GetMaxHp();
    }
}