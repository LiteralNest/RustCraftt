using UnityEngine;

public class LootObject : MonoBehaviour, IDamagable
{
    [SerializeField] private float _hp = 100;

    private void CheckHp()
    {
        if(_hp <= 0)
            Destroy(gameObject);
    }

    public void GetDamage(int damage)
    {
        _hp -= damage;
        CheckHp();
    }
}
