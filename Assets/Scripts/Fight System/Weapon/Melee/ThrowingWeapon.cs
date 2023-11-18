using UnityEngine;

public class ThrowingWeapon : MonoBehaviour
{
    [Header("Attached Compontents")] [SerializeField]
    private Rigidbody _rb;

    [SerializeField] private Collider _collider;

    [Header("Main Params")] [SerializeField]
    private float _lerpSpeed = 2f;

    public void Throw(float force)
    {
        InventoryHandler.singleton.CharacterInventory.RemoveItemCountServerRpc(
            InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.InventoryCell.Item.Id, 1);

        _rb.AddForce(Camera.main.transform.forward * force, ForceMode.Impulse);
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
        transform.SetParent(other.transform);
    }
}