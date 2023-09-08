using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Ammo : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _torque;

    private void Start()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody>();
    }

    public void Fly(Vector3 force)
    {
        _rb.isKinematic = false;
        _rb.AddForce(force, ForceMode.Impulse);
        _rb.AddTorque(transform.right * _torque);
        transform.SetParent(null);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(_rb);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<IDamagable>(out var damagable)) return;
        damagable.GetDamage(_damage);
    }
}
