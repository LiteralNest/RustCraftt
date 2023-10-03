using UnityEngine;

namespace SurroundingEffectsSystem
{
    public class TemperatureZone : MonoBehaviour
    {
        public SurroundingEffectsStateType EffectType;
        [SerializeField] private Color _color;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = _color; 
            Gizmos.DrawWireSphere(transform.position, 20f);
        }
    }
    
    
}