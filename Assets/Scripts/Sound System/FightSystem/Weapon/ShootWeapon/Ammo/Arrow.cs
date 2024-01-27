using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace FightSystem.Weapon.ShootWeapon.Ammo
{
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
            // StartCoroutine(DespawnObject());
        }
        
        public void ArrowFly(Vector3 force)
        {
            _rb.isKinematic = false;
            _rb.AddForce(force, ForceMode.Impulse);
            _rb.useGravity = true;
            _rb.AddTorque(_rb.transform.forward * _torque);
        }

        private void OnCollisionEnter(Collision other)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            if (IsServer)
            {
                GetComponent<NetworkObject>().TrySetParent(other.transform);
                StartCoroutine(DespawnObject());
            }
           
        }

        
        private IEnumerator DespawnObject()
        {
            if (!IsServer) yield break;
            yield return new WaitForSeconds(_despawnTime); 
            Destroy(gameObject);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}