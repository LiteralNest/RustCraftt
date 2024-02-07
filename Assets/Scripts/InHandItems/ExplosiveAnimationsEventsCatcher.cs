using UnityEngine;

namespace InHandItems
{
    public class ExplosiveAnimationsEventsCatcher : MonoBehaviour
    {
        [SerializeField] private InHandExplosive _explosive;
        
        public void SpawnPrefab()
            => _explosive.SpawnPrefab();
    }
}