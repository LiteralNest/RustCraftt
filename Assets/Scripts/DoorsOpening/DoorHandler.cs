using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private DoorLocker _doorLocker;
    private static readonly int Opened = Animator.StringToHash("Opened");

    private void Start()
        => gameObject.tag = "Door";

    public void Open()
    {
        if(_doorLocker.TargetLock) return;
        _anim.SetBool(Opened, !_anim.GetBool(Opened));
    }
}