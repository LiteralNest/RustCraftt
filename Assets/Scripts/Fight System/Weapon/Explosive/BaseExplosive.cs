using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public abstract class BaseExplosive : NetworkBehaviour
{
    [Header("Attached Scripts")] [SerializeField]
    protected AudioSource _explosiveSource;

    [SerializeField] protected AudioClip _explosiveClip;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _explosionVfx;

    [Header("Main Params")] [SerializeField]
    protected float _explosionRadius = 5f;

    [SerializeField] protected float _maxDamage = 50f;

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

    private void DamageObjects()
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
            damageable.Shake();
            damageable.GetDamage((int)damage, false);
        }
    }

    private async Task PlaySound()
    {
        _explosiveSource.PlayOneShot(_explosiveClip);
        await Task.Delay((int)(_explosiveClip.length * 1000));
    }

    protected void ShakeCamera()
    {
        if (_cameraShake != null)
        {
            _cameraShake.StartShake(shakeDuration, shakeMagnitude);
        }
    }

    private async void Explode()
    {
        _explosionVfx.SetActive(true);
        _model.SetActive(false);
        DamageObjects();
        await PlaySound();
        Destroy(gameObject);
        if (IsServer)
            GetComponent<NetworkObject>().Despawn();
    }

    [ServerRpc(RequireOwnership = false)]
    protected void ExplodeServerRpc()
    {
        if (!IsServer) return;
        Explode();
    }
}