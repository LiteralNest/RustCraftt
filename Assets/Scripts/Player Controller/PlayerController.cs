using Animation_System;
using OnPlayerItems;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Attached Scripts")] [SerializeField]
        private InHandObjectsContainer _inHandObjectsContainer;

        [Header("Move")] [SerializeField] private float _movingSpeed = 5;
        [SerializeField] private CharacterController _controller;
        public bool IsMoving { get; private set; }
        private Vector2 _move;

        [Header("Run")] [SerializeField] private float _runningKoef = 1.5f;
        private bool _isRunning;

        [Header("Swim")] [SerializeField] private float _swimSpeed = 3f;

        [Header("Gravity")] [SerializeField] private PlayerJumper _jump;
        private bool _isCrouching;
        public bool IsCrouching => _isCrouching;
        public bool IsSwimming { get; set; }

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
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

        #region Animation

        public void HandleCrouching(bool value)
        {
            if (value)
                AnimationsManager.Singleton.SetStartCrouching();
            else if (_isCrouching)
                AnimationsManager.Singleton.SetStopCrouching();
            _isCrouching = value;
        }


        private void HandleWalkAnimation()
        {
            if (AnimationsManager.Singleton == null) return;
            if (_isCrouching)
                AnimationsManager.Singleton.SetCrouch();
            else
                AnimationsManager.Singleton.SetWalk();
        }

        #endregion

        #region Movement

        private void Move()
        {
            Vector3 moveDirection = new Vector3(_move.x, 0f, _move.y);

            if (!_isRunning && _move != Vector2.zero)
            {
                moveDirection = transform.TransformDirection(moveDirection);
                HandleWalkAnimation();
                _inHandObjectsContainer.SetWalk(true);
                _controller.Move(moveDirection * _movingSpeed * Time.deltaTime);
            }
            else if (_isRunning)
            {
                _controller.Move(_camera.transform.forward * _movingSpeed * _runningKoef * Time.deltaTime);
                _inHandObjectsContainer.SetWalk(true);
                HandleWalkAnimation();
                return;
            }


            if (_move != Vector2.zero) return;
            if (AnimationsManager.Singleton != null)
                AnimationsManager.Singleton.SetIdle();
            _inHandObjectsContainer.SetWalk(false);
        }

        public void StartRunning()
        {
            _inHandObjectsContainer.SetRun(true);
            _isRunning = true;
        }

        public void StopRunning()
        {
            if (AnimationsManager.Singleton != null)
                AnimationsManager.Singleton.SetIdle();
            _inHandObjectsContainer.SetRun(false);
            _isRunning = false;
        }

        #endregion

        #region Swimming

        private void Swim()
        {
            Vector3 moveDirection = new Vector3(_move.x, 0f, _move.y);

            if (moveDirection != Vector3.zero)
            {
                moveDirection = _camera.transform.TransformDirection(moveDirection);
                _controller.Move(moveDirection * _swimSpeed * Time.deltaTime);
            }
        }

        #endregion

        #region Collision Handling

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                IsSwimming = true;
                _jump.enabled = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                IsSwimming = false;
                _jump.enabled = true;
            }
        }

        #endregion
    }
}