using System.Collections;
using Building_System;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Player_Controller
{
    [RequireComponent(typeof(PlayerJumper))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerClimber : NetworkBehaviour
    {
        [SerializeField] private NetworkObject _playerNetworkObject;
        [SerializeField] private float _climbForce = 10.0f;
        [SerializeField] private float _raycastDistance = 0.3f;
        [SerializeField] private Transform _raycastOrigin;
        [SerializeField] private LayerMask _rayCastMask;
        
        private PlayerJumper _jumper;

        private Ladder _currentLadder;
        private bool _climbButtonPressed;

        private void Start()
        {
            _jumper = GetComponent<PlayerJumper>();
        }

        private void Update()
        {
            if (CharacterUIHandler.singleton == null) return;
            if (!_playerNetworkObject.IsOwner) return;
            bool ladderFound = LadderFound();
            if (!ladderFound)
                _climbButtonPressed = false;
            AssignGravity();
            CharacterUIHandler.singleton.HandleMovingUpButton(ladderFound);
        }

        private void FixedUpdate()
        {
            if (!_climbButtonPressed) return;
            TryClimb(_jumper.transform, _currentLadder);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(_raycastOrigin.position, _raycastOrigin.forward * _raycastDistance);
        }

#endif

        public void AssignClimbing(bool value)
        {
            if (value)
                _jumper.CanUseGravity = false;

            _climbButtonPressed = value;
        }

        private void AssignGravity()
            => _jumper.CanUseGravity = !_currentLadder;

        private bool LadderFound()
        {
            _currentLadder = null;
            if (!Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward, out RaycastHit hit,
                    _raycastDistance, _rayCastMask)) return false;
            if (!hit.collider.CompareTag("DamagingItem") && !hit.collider.CompareTag("Ladder")) return false;
            if (!hit.collider.TryGetComponent(out Ladder ladder)) return false;
            _currentLadder = ladder;
            return true;
        }

        private void TryClimb(Transform player, Ladder ladder)
        {
            if (!_currentLadder) return;
            Vector3 climbDirection = ladder.transform.up;
            player.position += climbDirection * _climbForce * Time.deltaTime;
            _jumper.MoveWithLadder(climbDirection * _climbForce * Time.deltaTime);
        }
    }
}