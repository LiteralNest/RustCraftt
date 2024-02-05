using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Building_System.Building.Placing_Objects.GroundChecker
{
    [RequireComponent(typeof(NetworkObject))]
    public class PlacingObjectGroundHandler : NetworkBehaviour
    {
        [SerializeField] private LayerMask _rayCastLayer;
        [SerializeField] private List<PlacingObjectGroundChecker> _checkers;
        private NetworkObject _root;
        private List<IDestroyable> _targets = new List<IDestroyable>();

        private void Start()
        {
            _root = GetComponent<NetworkObject>();
            foreach(var checker in _checkers)
                checker.Init(_rayCastLayer, this);
        }

        public void AddTarget(IDestroyable target)
        {
            _targets.Add(target);
            target.OnDestroyed += TargetDestroyed;
        }

        private void TargetDestroyed(IDestroyable target)
        {
            _targets.Remove(target);
            if (_targets.Count == 0)
                _root.Despawn();
        }
    }
}