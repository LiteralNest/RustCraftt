using UnityEngine;

public abstract class BaseExplosive : MonoBehaviour
{
    [SerializeField] protected float _explosionRadius = 5f;
    [SerializeField] protected float _maxDamage = 50f;
    protected Collider[] _colliders;
    
    protected bool _hasExploded = false;

    protected virtual void Start()
    {
        _colliders = new Collider[100];
    }

    protected void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;

        var numColliders = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _colliders);

        for (var i = 0; i < numColliders; i++)
        {
            var damageable = _colliders[i].GetComponent<IDamagable>();
            if (damageable == null) continue;

            var distance = Vector3.Distance(transform.position, _colliders[i].transform.position);
            var damage = Mathf.Lerp(_maxDamage, 0f, distance / _explosionRadius);
            damageable.GetDamage((int)damage);
        }

        Destroy(gameObject);
    }
}