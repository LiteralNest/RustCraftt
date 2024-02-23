using UnityEngine;
using UnityEngine.Serialization;

namespace Test
{
    public class ForwardDirectionView : MonoBehaviour
    {
        #if UNITY_EDITOR

        [SerializeField] private Color _gizmosColor = Color.yellow;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawRay(transform.position, transform.forward * 4);
        }
        
        #endif
    }
}