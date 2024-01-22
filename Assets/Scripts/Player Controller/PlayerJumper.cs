using System;
using Animation_System;
using UnityEngine;

namespace Player_Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerJumper : MonoBehaviour
    {
        [Header("Attached Scripts")]
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private float Gravity = -9.8f;

        [Header("Main Params")]
        [SerializeField] private float _jumpForce = 20f;
        [SerializeField] private float _damping = 0.5f;

        private bool _canUseGravity = true;
        private Vector3 _velocity;
        

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            UpdateGravity();
        }

        public void Jump()
        {
            AnimationsManager.Singleton.SetJump();

            if (IsGrounded())
            {
                _velocity.y = Mathf.Sqrt(_jumpForce * _damping * -2f * Gravity); 
            }
        }

        private void UpdateGravity()
        {
            if (!_canUseGravity)
            {
                _velocity.y = 0f;
            }
            else
            {
                _velocity.y += Gravity * Time.deltaTime;
            }

            _characterController.Move(_velocity * Time.deltaTime);
        }
        
        public bool IsGrounded()
        {
            return _characterController.isGrounded;
        }
        
        public void SetGravity(bool enableGravity)
        {
            _canUseGravity = enableGravity;
        }
        
        public void MoveWithLadder(Vector3 moveDirection)
        {
            _characterController.Move(moveDirection);
        }
    }
}