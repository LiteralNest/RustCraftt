using UnityEngine;

namespace Test
{
    public class ForwardDirectionView : MonoBehaviour
    {
        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * 4);
        }
        
        #endif
    }
}