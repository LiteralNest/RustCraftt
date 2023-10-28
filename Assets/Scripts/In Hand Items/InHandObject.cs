using System.Collections.Generic;
using UnityEngine;

public class InHandObject : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private Animator _animator;

    [Header("Start Init")] [SerializeField]
    private List<Renderer> _renderers;
    [SerializeField] private List<GameObject> _activatingObjects;
    
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

    public void DisplayRenderers(bool value)
    {
        foreach(var renderer in _renderers)
            renderer.enabled = value;
        foreach(var activatingObject in _activatingObjects)
            activatingObject.SetActive(value);
    }
}