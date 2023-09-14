using UnityEngine;

public abstract class DamagableBuilding : MonoBehaviour, IDamagable
{
    [field:SerializeField] public int Hp { get; protected set; }
    
    public void GetDamage(int damage)
    {
        Hp -= damage;
    }
}
