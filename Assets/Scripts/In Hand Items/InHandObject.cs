using UnityEngine;

public class InHandObject : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private Animator _animator;

    [Header("Animator States")]
    [SerializeField] private string _attackIndex = "Attacking";
    [SerializeField] private string _walkIndex = "Walking";
    [SerializeField] private string _runIndex = "Running";

    public void Walk(bool value)
    {
        if(!_animator) return;
        _animator.SetBool(_runIndex, false);
        _animator.SetBool(_walkIndex, value);
    }

    public void Run(bool value)
    {
        if(!_animator) return;
        _animator.SetBool(_runIndex, value);
        _animator.SetBool(_walkIndex, false);
    }

    public void HandleAttacking(bool attack)
    {
        if(!_animator) return;
        _animator.SetBool(_attackIndex, attack);
    }
}