using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class DamageObject : MonoBehaviour
{
   [SerializeField] private Rigidbody _rb;
   [SerializeField] private int _damage = 25;
   [SerializeField] private bool _destroyOnEnter;
   private void Start()
   {
      if(!_rb)
         _rb = GetComponent<Rigidbody>();
      _rb.useGravity = false;
   }
   
   private void OnTriggerEnter(Collider other)
   {
      if (!other.gameObject.TryGetComponent<IDamagable>(out var damagable)) return;
      damagable.GetDamage(_damage);
      if(_destroyOnEnter) Destroy(gameObject);
   }
}
