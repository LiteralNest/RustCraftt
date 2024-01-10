using UnityEngine;

namespace Vehicle
{
    public abstract class BaseVehicle : MonoBehaviour
    {
        [SerializeField] protected CharacterController VehicleController;
        [SerializeField] protected float Gravity = -9.8f;
        [SerializeField] protected float MoveSpeed = 5f;
        [SerializeField] protected float RotationSpeed = 40f;
    }
}
