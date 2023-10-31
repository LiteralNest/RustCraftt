using Unity.Netcode;
using UnityEngine;

public class DoorHandler : NetworkBehaviour
{
    [field: SerializeField] public Transform MainTransform { get; private set; }
    [SerializeField] private Animator _anim;
    public KeyLocker DoorLocker { get; set; }
    private static readonly int Opened = Animator.StringToHash("Opened");

    private void Start()
        => gameObject.tag = "Door";

    public void Open(int id)
    {
        if(!DoorLocker.CanBeOpened(id)) return;
        _anim.SetBool(Opened, !_anim.GetBool(Opened));
    }
}