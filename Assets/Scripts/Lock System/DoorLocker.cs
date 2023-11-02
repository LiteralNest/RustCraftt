using UnityEngine;

public class DoorLocker : MonoBehaviour
{
    [SerializeField] private DoorHandler _fatherDoor;
    public Transform TargetLock { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Lock")) return;
        TargetLock = other.transform;
        TargetLock.SetParent(_fatherDoor.MainTransform);
        var locker = other.GetComponent<KeyLocker>();
        _fatherDoor.DoorLocker = locker;
        Destroy(gameObject);
    }
}
