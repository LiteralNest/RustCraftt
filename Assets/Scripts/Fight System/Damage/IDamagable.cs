public interface IDamagable
{
    public ushort GetHp();
    public int GetMaxHp();
    public void GetDamage(int damage, bool playSound = true);
    public void Destroy();
    public void Shake();
}