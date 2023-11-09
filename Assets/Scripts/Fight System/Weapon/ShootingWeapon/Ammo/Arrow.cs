using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Fight_System.Weapon.ShootingWeapon.Ammo
{
    [RequireComponent(typeof(WeaponSoundPlayer))]
    public class Arrow : NetworkBehaviour
    {
        [field: SerializeField] public int AmmoPoolId { get; private set; }
        [SerializeField] private float _despawnTime;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _torque = 10f;
        
        private void Start()
        {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();
            StartCoroutine(DespawnObject());
        }
        
        public void ArrowFly(Vector3 force)
        {
            _rb.isKinematic = false;
            _rb.AddForce(force, ForceMode.Impulse);
            _rb.AddTorque(_rb.transform.right * _torque);
            transform.SetParent(null);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.isKinematic = true;
            transform.SetParent(other.transform);  
            // Destroy(_rb);
        }
        
        private IEnumerator DespawnObject()
        {
            yield return new WaitForSeconds(_despawnTime);
            Destroy(gameObject);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}