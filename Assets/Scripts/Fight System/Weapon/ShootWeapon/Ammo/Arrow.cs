using System.Collections;
using UnityEngine;

namespace Fight_System.Weapon.ShootWeapon.Ammo
{
    [RequireComponent(typeof(WeaponSoundPlayer))]
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
           
            float angleY = Mathf.Atan2(force.x, force.z) * Mathf.Rad2Deg;
            _rb.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angleY, transform.rotation.eulerAngles.z);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.isKinematic = true;

            // Set the arrow's position and rotation to match the point of impact
            transform.position = other.contacts[0].point;

            // Родительство оставляем как есть
            transform.SetParent(other.transform);
            StartCoroutine(DespawnObject());
        }
        
        private IEnumerator DespawnObject()
        {
            yield return new WaitForSeconds(_despawnTime); Destroy(gameObject);
            // GetComponent<NetworkObject>().Despawn();
        }
    }
}