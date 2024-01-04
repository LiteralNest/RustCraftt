using UnityEngine;

namespace Map
{
    public class MapCamera : MonoBehaviour
    {
        public static MapCamera Singleton { get; private set; }
        public Camera TargetCamera;
        private void Awake()
            => Singleton = this;
    }
}