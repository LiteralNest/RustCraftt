using System;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    [field:SerializeField] public Vector3 TargetRotation;

    private void Awake()
        => gameObject.tag = "Snap";
}
