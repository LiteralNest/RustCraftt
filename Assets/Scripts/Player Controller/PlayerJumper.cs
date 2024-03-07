using System.Collections.Generic;
using System.Collections;
using Animation_System;
using CharacterStatsSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player_Controller
{
    [System.Serializable]
    public class FallDamageRange
    {
        public float minHeight;
        public float maxHeight;
        public int damageAmount;
    }

    [RequireComponent(typeof(CharacterController))]
    public class PlayerJumper : NetworkBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private CharacterController _characterController;

        [Header("Main Params")] [SerializeField]
        private float _gravity = -9.8f;

        [Range(0, 10)] [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _damping = 1.15f;
        [SerializeField] private float _gravitySnap = -6f;

        [Header("Fall Damage Ranges")] [SerializeField]
        private List<FallDamageRange> _fallDamageRanges;

        private Vector3 _velocity;
        private float _fallStartHeight;

        public bool CanUseGravity { private get; set; }
        private bool _cachedGroundedPos;

        private void Start()
        {
            CanUseGravity = true;
            _fallStartHeight = transform.position.y;
        }

        private void Update()
        {
            CheckObjectFly();
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            UpdateGravity();
        }

        public void Jump()
        {
            if (!_characterController.isGrounded) return;
            AnimationsManager.Singleton.SetJump();
            _velocity.y = Mathf.Sqrt(_jumpForce * _damping * -2f * _gravity);
        }

        private void CheckObjectFly()
        {
            if (_characterController.isGrounded == _cachedGroundedPos) return;
            _cachedGroundedPos = _characterController.isGrounded;
            if (!_cachedGroundedPos)
                _fallStartHeight = transform.position.y;
            else
                CheckFallDamage();
        }

        private void UpdateGravity()
        {
            if (!CanUseGravity) return;
            if (_characterController.isGrounded && _velocity.y < 0.0f)
            {
                _velocity.y = _gravitySnap;
            }
            else
                _velocity.y += 3f * _gravity * Time.deltaTime;

            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void CheckFallDamage()
        {
            if(!IsOwner || IsServer) return;
            var fallHeight = _fallStartHeight - transform.position.y;

            foreach (var fallDamageRange in _fallDamageRanges)
            {
                if (fallHeight > fallDamageRange.minHeight && fallHeight <= fallDamageRange.maxHeight)
                {
                    CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Health, fallDamageRange.damageAmount);
                    break;
                }
            }
        }

        public void MoveWithLadder(Vector3 moveDirection)
        {
            _characterController.Move(moveDirection);
        }
    }
}