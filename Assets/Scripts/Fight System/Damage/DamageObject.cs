using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class DamageObject : MonoBehaviour
{
   [SerializeField] private int _damage = 25;
   [SerializeField] private bool _destroyOnEnter;
   
   private void OnTriggerEnter(Collider other)
   {
      if (!other.gameObject.TryGetComponent<IDamagable>(out var damagable)) return;
      damagable.GetDamage(_damage);
      if(_destroyOnEnter) Destroy(gameObject);
   }
}
