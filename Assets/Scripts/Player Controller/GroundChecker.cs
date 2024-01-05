using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private List<string> _availableTriggers = new List<string>();
    [field: SerializeField] public bool IsGrounded { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        if(_availableTriggers.Contains(other.tag))
            IsGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(_availableTriggers.Contains(other.tag))
            IsGrounded = false;
    }
}