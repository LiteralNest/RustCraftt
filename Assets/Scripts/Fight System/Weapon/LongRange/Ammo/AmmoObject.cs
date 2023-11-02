using System.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AmmoObject : NetworkBehaviour
{
    [field: SerializeField] public int AmmoPoolId { get; private set; }
    [SerializeField] private float _despawnTime;
    [SerializeField] private Rigidbody _rb;

    private void Start()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody>();
        StartCoroutine(DespawnObject());
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(_rb);
    }

    private IEnumerator DespawnObject()
    {
        yield return new WaitForSeconds(_despawnTime);
        Destroy(gameObject);
        GetComponent<NetworkObject>().Despawn();
    }
}
