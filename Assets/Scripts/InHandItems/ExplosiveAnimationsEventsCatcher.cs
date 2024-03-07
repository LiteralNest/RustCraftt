using UnityEngine;

namespace InHandItems
{
    public class ExplosiveAnimationsEventsCatcher : MonoBehaviour
    {
        [SerializeField] private InHandExplosive _explosive;
        [SerializeField] private GameObject _explosiveObject;

        public void SpawnPrefab()
        {
            if (_explosiveObject.activeSelf)
                _explosive.SpawnPrefab();
        }
    }
}