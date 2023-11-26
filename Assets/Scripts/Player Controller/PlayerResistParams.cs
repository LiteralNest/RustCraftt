using UnityEngine;

namespace Player_Controller
{
    public class PlayerResistParams : MonoBehaviour
    {
        [SerializeField] private float _coldResist = 0.5f;
        [SerializeField] private float _radiationResist = 0.5f;

        public float ColdResist => _coldResist;
        public float RadiationResist => _radiationResist;
    }
}