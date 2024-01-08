using Animation_System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJumper : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float Gravity = -9.8f;
    [Header("Main Params")]
    [SerializeField] private float _jumpForce = 20f;

    private Vector3 _velocity;
    
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        UpdateGravity();
    }

    private void Jump()
    {
        AnimationsManager.Singleton.SetJump();

        if (_characterController.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * Gravity);
        }
    }

    private void UpdateGravity()
    {
        _velocity.y += Gravity * Time.deltaTime;

        _characterController.Move(_velocity * Time.deltaTime);
    }
}