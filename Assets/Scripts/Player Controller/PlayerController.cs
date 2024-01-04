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

        [Header("Move")] [SerializeField] private NetworkVariable<float> _movingSpeed =
            new(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public bool IsMoving { get; private set; }
        private Vector2 _move;

        [Header("Run")] [SerializeField] private float _runningKoef = 1.5f;
        private bool _ifRunning;
        private float _currentMovingSpeed;

        [Header("Swim")] [SerializeField] private float _swimSpeed = 0.5f;
        public bool IsSwimming { get; set; }

        private Rigidbody _rb;
        private float _originalDrag;
        private float _originalAngularDrag;

        private readonly float _targetDrag = 0.3f;
        private readonly float _targetAngularDrag = 0.7f;
        private readonly float _floatStrength = 0.4f;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _rb = GetComponent<Rigidbody>();

            _originalDrag = _rb.drag;
            _originalAngularDrag = _rb.angularDrag;
            _currentMovingSpeed = _movingSpeed.Value;
        }


        private void FixedUpdate()
        {
            if (IsSwimming != true)
            {
                if (!_rb.useGravity)
                    ReturnOriginalRb();
                if (!_ifRunning)
                {
                    Move();
                    return;
                }

                transform.Translate(Vector3.forward * _currentMovingSpeed * Time.deltaTime, Space.Self);
                return;
            }

            Swim();
        }

        private void ReturnOriginalRb()
        {
            _rb.useGravity = true;
            _rb.drag = _originalDrag;
            _rb.angularDrag = _originalAngularDrag;
        }

        #region InputMap

        public void OnMove(InputAction.CallbackContext context)
        {
            if (IsSwimming)
            {
                IsMoving = true;
                _move = context.ReadValue<Vector2>();
            }
            else
            {
                IsMoving = true;
                _move = context.ReadValue<Vector2>();
                IsMoving = false;
            }
        }

        #endregion

        #region Movement

        private void Move()
        {
            Vector3 movement = new Vector3(_move.x, 0f, _move.y);
            if (movement != Vector3.zero)
            {
                AnimationsManager.Singleton.SetWalk();
                transform.Translate(movement * _currentMovingSpeed * Time.deltaTime, Space.Self);
                return;
            }

            _inHandObjectsContainer.SetWalk(false);
            if(AnimationsManager.Singleton != null)
                AnimationsManager.Singleton.SetIdle();
        }

        public void StartRunning()
        {
            _inHandObjectsContainer.SetRun(true);
            AnimationsManager.Singleton.SetWalk();
            _ifRunning = true;
            _currentMovingSpeed *= _runningKoef;
        }

        public void StopRunning()
        {
            AnimationsManager.Singleton.SetIdle();
            _inHandObjectsContainer.SetRun(false);
            _ifRunning = false;
            _currentMovingSpeed = _movingSpeed.Value;
        }

        #endregion

        #region Swimming

        private void Swim()
        {
            if (_rb.useGravity)
                _rb.useGravity = false;
            var cameraForward = _camera.transform.forward;
            cameraForward *= _move.y;
            Vector3 movement = new Vector3(_move.x, cameraForward.y, _move.y);
            // cameraForward.x = _move.x;
            cameraForward.Normalize();


            if (cameraForward != Vector3.zero)
            {
                _rb.drag = _targetDrag;
                _rb.angularDrag = _targetAngularDrag;
                transform.Translate(movement * _swimSpeed * Time.deltaTime, Space.Self);
            }
        }


        private void FloatUp()
        {
            if (IsSwimming)
            {
                Vector3 jumpForce = Vector3.up * _floatStrength;
                _rb.AddForce(jumpForce, ForceMode.Force);
            }
        }

        #endregion
    }
}