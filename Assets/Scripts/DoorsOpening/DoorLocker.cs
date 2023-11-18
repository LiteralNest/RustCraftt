using UnityEngine;

public class DoorLocker : MonoBehaviour
{
    [SerializeField] private Transform _fatherDoor;
    public Transform TargetLock { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Lock")) return;
        TargetLock = other.transform;
        TargetLock.SetParent(_fatherDoor);
    }
}
