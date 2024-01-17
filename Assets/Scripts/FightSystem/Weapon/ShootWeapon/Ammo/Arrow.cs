using System.Collections;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon.Ammo
{
    public class Arrow : MonoBehaviour
    {
        [field: SerializeField] public int AmmoPoolId { get; private set; }
        [SerializeField] private float _despawnTime;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _torque = 10f;
        [SerializeField] private Collider _arrowCollider;
        
        private void Start()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();
            _arrowCollider.enabled = false;
            // StartCoroutine(DespawnObject());
        }
        
        public void ArrowFly(Vector3 force)
        {
            _arrowCollider.enabled = true;
            _rb.isKinematic = false;
            _rb.AddForce(force, ForceMode.Impulse);
            _rb.useGravity = true;
            _rb.AddTorque(_rb.transform.forward * _torque);
        }

        private void OnCollisionEnter(Collision other)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.SetParent(other.transform);

            StartCoroutine(DespawnObject());
        }

        
        private IEnumerator DespawnObject()
        {
            yield return new WaitForSeconds(_despawnTime); 
            Destroy(gameObject);  
            //GetComponent<NetworkObject>().Despawn();
        }
    }
}