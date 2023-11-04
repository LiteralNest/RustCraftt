using UnityEngine;

public class GunTrap : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 5f;
    [SerializeField] private float _rayDistance = 5f;
    [SerializeField] private LayerMask _playerMask;

    private Transform _transform;
    private Ray _ray;
    
    private void Start()
    {
        _transform = transform;
        _ray = new Ray(_transform.position, _transform.forward);
    }

    private void Update()
    {
        Debug.DrawRay(_transform.position, _transform.forward * _rayDistance, Color.green);

        if (!Physics.Raycast(_ray, out var hit, _rayDistance, _playerMask)) return;
        if (hit.collider.CompareTag("Player"))
        {
            CharacterStats.Singleton.MinusStat(CharacterStatType.Health, _damageAmount);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_transform.position, _transform.forward * _rayDistance);
    }
}