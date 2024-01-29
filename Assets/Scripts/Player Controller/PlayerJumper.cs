using System.Collections.Generic;
using Animation_System;
using Character_Stats;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class FallDamageRange
{
    public float minHeight;
    public float maxHeight;
    public float damageAmount;
}

[RequireComponent(typeof(CharacterController))]
public class PlayerJumper : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CharacterStats _characterStats;

    [Header("Main Params")]
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _damping = 1.15f;
    [SerializeField] private float _gravitySnap = -6f;

    [Header("Fall Damage Ranges")]
    [SerializeField] private List<FallDamageRange> _fallDamageRanges;

    private bool _canUseGravity = true;
    private Vector3 _velocity;
    private float _fallStartHeight;

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
            _fallStartHeight = transform.position.y;
            _velocity.y = Mathf.Sqrt(_jumpForce * _damping * -2f * _gravity);
        }
    }

    private void UpdateGravity()
    {
        if (!_canUseGravity && IsGrounded() && _velocity.y < 0.0f)
        {
            _velocity.y = _gravitySnap;
        }
        else
        {
            _velocity.y += 3f * _gravity * Time.deltaTime;

            if (_velocity.y < -1f)
            {
                _velocity.y = _gravitySnap;
            }

            CheckFallDamage();
        }

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void CheckFallDamage()
    {
        var fallHeight = _fallStartHeight - transform.position.y;

        foreach (var fallDamageRange in _fallDamageRanges)
        {
            if (fallHeight > fallDamageRange.minHeight && fallHeight <= fallDamageRange.maxHeight)
            {
                Debug.Log($"Fall Damage: {fallDamageRange.damageAmount}");
                _characterStats.MinusStat(CharacterStatType.Health, fallDamageRange.damageAmount);
                break;
            }
        }
    }

    private bool IsGrounded()
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
