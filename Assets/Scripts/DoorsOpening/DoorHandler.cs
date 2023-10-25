using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    private static readonly int Opened = Animator.StringToHash("Opened");

    private void Start()
        => gameObject.tag = "Door";
    
    public void Open()
        => _anim.SetBool(Opened, !_anim.GetBool(Opened));
}