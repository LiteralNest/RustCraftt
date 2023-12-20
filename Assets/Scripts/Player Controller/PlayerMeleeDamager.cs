using UnityEngine;

namespace Player_Controller
{
    public class PlayerMeleeDamager : MonoBehaviour
    {
        [SerializeField] private float _damageRange = 1f;
        [SerializeField] private LayerMask _damageLayer;
    
        
        
        public bool TryDamage(int damageAmount)
        {
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var hit, _damageRange, _damageLayer))
            {
                var damagable = hit.collider.GetComponent<IDamagable>();
                if(damagable == null) return false;
                damagable.GetDamage(damageAmount);
                return true; 
            }
            return false; 
        }
    }
}