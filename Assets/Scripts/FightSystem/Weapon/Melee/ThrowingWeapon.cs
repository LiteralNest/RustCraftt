using UnityEngine;

namespace FightSystem.Weapon.Melee
{
    public class ThrowingWeapon : MonoBehaviour
    {
        [Header("Attached Compontents")] 
        [SerializeField] private Rigidbody _rb;

        [Header("Main Params")] [SerializeField]
        private float _lerpSpeed = 2f;

        public void Throw(Vector3 direction, float force)
        {
            _rb.AddForce(direction * force, ForceMode.Impulse);
            Rotate();
        }

        private void Rotate()
        {
            var velocity = _rb.velocity.normalized;
            if (_rb.velocity.sqrMagnitude > 0.01f)
            {
                var newRotation = Quaternion.LookRotation(velocity, Vector3.down);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * _lerpSpeed);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.isKinematic = true;

            transform.position = other.contacts[0].point;
        }
    }
}