using UnityEngine;

namespace Vehicle
{
    public abstract class BaseVehicle : MonoBehaviour
    {
        [SerializeField] protected Rigidbody VehicleRb;
        [SerializeField] protected float MoveSpeed = 5f;
        [SerializeField] protected float RotationSpeed = 40f;
    }
}
