using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace StabilizationSystem.Objects
{
    public class ObjectStabilizator : MonoBehaviour
    {
        [Header("Attached Components")] [SerializeField]
        private NetworkObject _networkObject;

        [SerializeField] private List<ObjectStabilizatorSnap> _snaps = new List<ObjectStabilizatorSnap>();

        public void TryDestroy()
        {
            foreach (var snap in _snaps)
            {
                if (snap.StabilizationBlock) continue;
                _networkObject.Despawn();
            }
        }
    }
}