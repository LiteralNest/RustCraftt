using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Ore : MonoBehaviour
{
    [SerializeField] private float _recoveringSpeed;
    [SerializeField] protected Resource _targetResource;
    [SerializeField] private Renderer _renderer;

    private void Start()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
    }

    private async Task Recover()
    {
        await Task.Delay((int)(_recoveringSpeed * 1000));
        _renderer.enabled = true;
    }
    
    protected async Task Destroy()
    {
        _renderer.enabled = false;
        await Recover();
    }
}
