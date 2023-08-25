using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ResourceOre : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] private float _recoveringSpeed;
    [SerializeField] private Resource _targetResource;
    [SerializeField] private Renderer _renderer;
    private int _currentHp;

    private void Start()
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        _currentHp = _hp;
    }
    
    [ContextMenu("Minus HP")]
    public void MinusHp(InventorySlotsContainer inventory)
    {
        if(_currentHp <= 0) return;
        _currentHp--;
        inventory.AddItemToDesiredSlot(_targetResource, 2);
        if(_currentHp > 0) return;
        _renderer.enabled = false;
        Recover();
    }
    
    private async void Recover()
    {
        await Task.Delay((int)(_recoveringSpeed * 1000));
        _currentHp = _hp;
        _renderer.enabled = true;
    }
}