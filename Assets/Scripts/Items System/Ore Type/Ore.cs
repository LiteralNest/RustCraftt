using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class Ore : NetworkBehaviour
{
    [SerializeField] private float _recoveringSpeed;
    [SerializeField] protected Resource _targetResource;
    [SerializeField] private List<Renderer> _renderers;

    public bool Recovering { get; protected set; } = false;

    private void TurnRenderers(bool value)
    {
        foreach (var renderer in _renderers)
            renderer.enabled = value;
    }
    
    private async Task Recover()
    {
        Recovering = true;
        await Task.Delay((int)(_recoveringSpeed * 1000));
        Recovering = false;
        TurnRenderers(true);
    }
    
    protected async Task Destroy()
    {
        TurnRenderers(false);
        await Recover();
    }
}
