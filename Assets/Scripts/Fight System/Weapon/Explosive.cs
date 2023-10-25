using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _maxDamage = 100f;
    [SerializeField] private float _timeToExplode = 3f;

    private Collider[] _colliders;
    private float _countdownTimer;
    private bool _hasExploded = false;

    private void Start()
    {
        _colliders = new Collider[100];
        _countdownTimer = _timeToExplode;
    }

    private void Update()
    {
        if (_hasExploded) return;
        _countdownTimer -= Time.deltaTime;

        if (_countdownTimer <= 0f) 
            Explode();
    }
    
    private void Explode()
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