using System.Collections;
using Fight_System.Weapon.ShootWeapon;
using UnityEngine;

public class MeleeShootingWeapon : MonoBehaviour
{
    [Header("ThrowingObject")] 
    [SerializeField] private GameObject _spearInHands;

    

    [Header("Physics")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _throwForce = 40f;

    [SerializeField] private Collider _spearCollider;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _layerMask;

    [Header("VisualEffects")] [SerializeField]
    private WeaponSoundPlayer _soundPlayer;
    [SerializeField] private GameObject _impactEffect;

    public bool IsInThrowingPosition { get;  private set; }
    private Vector3 _direction;
    private void Start()
    {
        gameObject.SetActive(false);
       
        _spearCollider.enabled = false;
        _rb.isKinematic = true;
        _rb.useGravity = false;
    }


    public void SetThrowingPosition()
    {
        _spearInHands.SetActive(false);
        gameObject.SetActive(true);

        IsInThrowingPosition = true;
        _spearCollider.enabled = true;
        _rb.useGravity = true;
    }


    private IEnumerator RotateSpearAfterPeak(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (_rb.velocity.y > 0)
        {
            float angleY = Mathf.Atan2(_rb.velocity.x, _rb.velocity.z) * Mathf.Rad2Deg;
            _rb.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angleY, transform.rotation.eulerAngles.z);
            yield return null;
        }
    }

    public void ThrowSpear()
    {
        IsInThrowingPosition = false;
        _spearCollider.enabled = false;

        _rb.isKinematic = false;
        _rb.useGravity = true;

        _direction = _spawnPoint.TransformDirection(Vector3.forward * _throwForce);
        _direction.Normalize();

        _rb.AddForce(_direction * _throwForce, ForceMode.Impulse);

        StartCoroutine(RotateSpearAfterPeak(2f)); // Начнем вращение через 0.5 секунды (можно настроить)
        transform.SetParent(null);
    }


    private void OnCollisionEnter(Collision other)
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;

        // Set the spear's position and rotation to match the point of impact
        // transform.position = other.contacts[0].point;
        // transform.rotation = Quaternion.LookRotation(other.contacts[0].normal);

        Debug.Log("Spear colided with" + other.collider.name);
        transform.SetParent(other.transform);

    }


    public void Attack(bool value)
    {
        GlobalEventsContainer.ShouldHandleAttacking?.Invoke(value);
    }
    
    private void OnDrawGizmos()
    {
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _direction * _throwForce);
        }
    }
}