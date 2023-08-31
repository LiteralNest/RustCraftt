using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] private float _recoveringSpeed;
    [SerializeField] protected Resource _targetResource;
    [SerializeField] private List<Renderer> _renderers;

    private void TurnRenderers(bool value)
    {
        foreach (var renderer in _renderers)
            renderer.enabled = value;
    }
    
    private async Task Recover()
    {
        await Task.Delay((int)(_recoveringSpeed * 1000));
        TurnRenderers(true);
    }
    
    protected async Task Destroy()
    {
        TurnRenderers(false);
        await Recover();
    }
}
