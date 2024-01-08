public interface IDamagable
{
    public int GetHp();
    public int GetMaxHp();
    public void GetDamage(int damage, bool playSound = true);
    public void Destroy();
    public void Shake();
}