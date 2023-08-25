using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AlertDisplayer : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    
    private void Start()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
        WaitForAnimationToEnd();
    }
    
    private async Task WaitForAnimationToEnd()
    {
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            await Task.Yield();
        Destroy(gameObject);
    }
}
