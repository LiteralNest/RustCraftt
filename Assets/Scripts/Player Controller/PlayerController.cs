using Animation_System;
using OnPlayerItems;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Attached Scripts")]
        [SerializeField] private InHandObjectsContainer _inHandObjectsContainer;

        [Header("Move")] 
        [SerializeField] private float _movingSpeed = 5;
        [SerializeField] private CharacterController _controller;
        public bool IsMoving { get; private set; }
        private Vector2 _move;

        [Header("Run")]
        [SerializeField] private float _runningKoef = 1.5f;
        private bool _ifRunning;
        private float _currentMovingSpeed;

        [Header("Swim")]
        [SerializeField] private float _swimSpeed = 0.5f;

        
        public bool IsSwimming { get; set; }

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _currentMovingSpeed = _movingSpeed;
        }

        private void Update()
        {
            if (IsSwimming)
            {
                Swim();
            }
            else
            {
                Move();
            }
        }
        
        private void ReturnOriginalController()
        {
            // for swimming behavior
        }

        #region InputMap

        public void OnMove(InputAction.CallbackContext context)
        {
            if (IsSwimming)
            {
                IsMoving = context.ReadValue<Vector2>().magnitude > 0.1f;
                _move = context.ReadValue<Vector2>();
            }
            else
            {
                IsMoving = context.ReadValue<Vector2>().magnitude > 0.1f;
                _move = context.ReadValue<Vector2>();
            }
        }

        #endregion

        #region Movement

        private void Move()
        {
            Vector3 moveDirection = new Vector3(_move.x, 0f, _move.y);
            moveDirection = transform.TransformDirection(moveDirection);

            if (!_ifRunning)
            {
                if(AnimationsManager.Singleton != null)
                    AnimationsManager.Singleton.SetWalk();
                _controller.SimpleMove(moveDirection * _movingSpeed);
            }
            else
            {
                _controller.SimpleMove(_camera.transform.forward * _movingSpeed * _runningKoef);
            }
          
            if(AnimationsManager.Singleton != null)
                AnimationsManager.Singleton.SetIdle();
            _inHandObjectsContainer.SetWalk(false);
            
        }

        public void StartRunning()
        {
            _inHandObjectsContainer.SetRun(true);
            _ifRunning = true;
        }

        public void StopRunning()
        {
            AnimationsManager.Singleton.SetIdle();
            _inHandObjectsContainer.SetRun(false);
            _ifRunning = false;
        }

        #endregion

        #region Swimming

        private void Swim()
        {
            if (_controller.isGrounded)
                _controller.Move(Vector3.down * 0.1f);  

            var cameraForward = _camera.transform.forward;
            cameraForward *= _move.y;
            Vector3 movement = new Vector3(_move.x, cameraForward.y, _move.y);
            cameraForward.Normalize();

            if (cameraForward != Vector3.zero)
            {
                _controller.Move(movement * _swimSpeed * Time.fixedDeltaTime);
            }
        }

        #endregion
    }
}
