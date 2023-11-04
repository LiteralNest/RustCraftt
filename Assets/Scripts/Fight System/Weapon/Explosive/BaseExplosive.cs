using UnityEngine;

public abstract class BaseExplosive : MonoBehaviour
{
    [SerializeField] protected float _explosionRadius = 5f;
    [SerializeField] protected float _maxDamage = 50f;
    
    [SerializeField] protected AudioSource _explosiveSource;
    [SerializeField] protected AudioClip _explosiveClip;

    [SerializeField] protected float shakeDuration = 0.5f;
    [SerializeField] protected float shakeMagnitude = 0.2f;
   
    protected CameraShake _cameraShake;
    protected Collider[] _colliders;
    protected bool _hasExploded = false;

    private Camera _camera;
    
    protected virtual void Start()
    {
        _colliders = new Collider[100];
        //How is better?
        _cameraShake = GetComponent<CameraShake>();
        _camera = Camera.main;
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

        PlaySound();
        ShakeCamera();

        Destroy(gameObject);
    }

    protected void PlaySound()
    {
        _explosiveSource.PlayOneShot(_explosiveClip); 
    }

    protected void ShakeCamera()
    {
        if (_cameraShake != null)
        {
            _cameraShake.StartShake(shakeDuration, shakeMagnitude);
        }
    }
}