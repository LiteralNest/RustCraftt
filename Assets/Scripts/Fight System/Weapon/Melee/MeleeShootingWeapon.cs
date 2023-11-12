using System;
using Fight_System.Weapon.ShootWeapon;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeShootingWeapon : MonoBehaviour
{
    [Header("ThrowingObject")] 
    [SerializeField] private GameObject _spearInHands;

    [Header("Physics")]
    [SerializeField] private float _throwForce = 40f;
    [SerializeField] private float _lerpSpeed = 2f;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Collider _spearCollider;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _layerMask;

    [Header("VisualEffects")] 
    [SerializeField] private WeaponSoundPlayer _soundPlayer;
    [SerializeField] private GameObject _impactEffect;

    public bool IsInThrowingPosition { get; private set; }
    private Vector3 _direction;
    private bool _rotateInFixedUpdate = false;

    private void OnEnable()
    {
        GlobalEventsContainer.WeaponMeleeObjectAssign?.Invoke(this);
    }

    private void Start()
    {
        // gameObject.SetActive(false);

        _rb.isKinematic = true;
        _rb.useGravity = false;
    }

    public void ThrowSpearByPhysic()
    {
        ThrowSpear();
        
        if (_rotateInFixedUpdate)
        {
            var velocity = _rb.velocity.normalized;
            Debug.Log(velocity.sqrMagnitude + " velocity");
            if (_rb.velocity.sqrMagnitude > 0.01f)
            {
                Debug.Log("1111");
                var newRotation = Quaternion.LookRotation(velocity, Vector3.down);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * _lerpSpeed);
            }

            _rotateInFixedUpdate = false;
        }
    }

    public void SetThrowingPosition()
    {
        _spearInHands.SetActive(false);
        gameObject.SetActive(true);

        IsInThrowingPosition = true;
        _spearCollider.enabled = true;
        _rb.useGravity = true;
        
    }


    private void OnDisable()
    {
        GlobalEventsContainer.WeaponMeleeObjectAssign?.Invoke(this);
        GlobalEventsContainer.ThrowMeleeButtonActivated?.Invoke(false);
    }

    private void ThrowSpear()
    {
        IsInThrowingPosition = false;
        _spearCollider.enabled = true;

        _rb.isKinematic = false;
        _rb.useGravity = true;
        
        _direction = _spawnPoint.forward * _throwForce;
        
        Debug.Log("Force applied: " + _direction);
        _rb.AddForce(_direction.normalized * _throwForce, ForceMode.Impulse);

        _rotateInFixedUpdate = true;
        transform.SetParent(null);
    }

    private void OnCollisionEnter(Collision other)
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;

        Debug.Log("Spear collided with " + other.collider.name);
        transform.position = other.contacts[0].point;
        transform.SetParent(other.transform);
    }


    // private void OnDrawGizmos()
    // {
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawRay(_spawnPoint.forward, _direction * _throwForce);
    //     }
    // }
    // public void Attack(bool value)
    // {
    //     GlobalEventsContainer.ShouldHandleAttacking?.Invoke(value);
    // }
}
