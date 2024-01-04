using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AlertDisplayer : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    private void Start()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        StartCoroutine(WaitForAnimationToEnd());
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;
        Destroy(gameObject);
    }
}