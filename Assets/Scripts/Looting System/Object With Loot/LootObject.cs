using UnityEngine;

public class LootObject : MonoBehaviour, IDamagable
{
    private int _cachedHp;
    [SerializeField] private int _hp = 100;

    private void Start()
    {
        transform.tag = "DamagingItem";
        _cachedHp = _hp;
    }

    private void CheckHp()
    {
        if (_hp <= 0)
            Destroy(gameObject);
    }

    public ushort GetHp()
        => (ushort)_hp;

    public int GetMaxHp()
        => _cachedHp;

    public void GetDamage(int damage)
    {
        _hp -= damage;
        CheckHp();
    }
}