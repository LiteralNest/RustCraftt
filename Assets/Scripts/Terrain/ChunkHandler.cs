using System;
using Building_System;
using UnityEngine;

namespace Terrain
{
    public class ChunkHandler : MonoBehaviour, IDestroyable
    {
        public Action<IDestroyable> OnDestroyed { get; set; }

        private void OnDestroy()
            => OnDestroyed?.Invoke(this);
    }
}