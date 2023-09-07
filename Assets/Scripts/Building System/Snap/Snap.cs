using UnityEngine;

public class Snap : MonoBehaviour
{
    [SerializeField] private string _tag = "WallSnap";
    [field:SerializeField] public Vector3 TargetRotation;

    private void Awake()
        => gameObject.tag = _tag;
}
