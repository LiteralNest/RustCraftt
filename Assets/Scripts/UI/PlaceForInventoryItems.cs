using UnityEngine;

namespace UI
{
    public class PlaceForInventoryItems : MonoBehaviour
    {
        public static PlaceForInventoryItems Singleton { get; private set; }

        private void Awake()
            => Singleton = this;
    }
}
