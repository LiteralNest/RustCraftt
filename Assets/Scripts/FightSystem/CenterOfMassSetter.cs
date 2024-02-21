using System;
using UnityEngine;

namespace FightSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class CenterOfMassSetter : MonoBehaviour
    {
        [SerializeField] private Vector3 _centerOfMass;

        private void Awake()
        {
            GetComponent<Rigidbody>().centerOfMass = _centerOfMass;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(_centerOfMass), 0.1f);
        }
#endif
    }
}