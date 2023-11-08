using System.Threading.Tasks;

public interface IWeapon
{
    void Attack(bool value);
    void Reload();
    bool CanReload();
}