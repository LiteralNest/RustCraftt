using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(ObjectsRayCaster))]
public class PlayerResourcesGatherer : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;
    
    [Header("Ore")]
    [SerializeField] private InventorySlotsContainer _inventory;
    [SerializeField] private float _recoveringSpeed = 0.3f;
    [SerializeField] private bool _canHit = true;
    
    [Header("Gathering")]
    private bool _gathering = false;

    private void Start()
    {
        if(_objectsRayCaster == null)
            _objectsRayCaster = GetComponent<ObjectsRayCaster>();
    }
    
    private void Update()
    {
        if(_gathering)
            TryHit();
    }
    
    private async void Recover()
    {
        _canHit = false;
        await Task.Delay((int)(_recoveringSpeed * 1000));
        _canHit = true;
    }

    public void StartGathering()
    {
        _gathering = true;
    }
    
    public void StopGathering()
    {
        _gathering = false;
    }
    
    private void TryHit()
    {
        if (!_canHit) return;
        Recover();
        var ore = _objectsRayCaster.TargetResourceOre;
        if(ore == null) return;
        ore.MinusHp(_inventory);
    }

    private bool TryOpenChest()
    {
        var chest = _objectsRayCaster.TargetBox;
        if(chest == null) return false;
        chest.Open();
        return true;
    }
    
    public void TryGather()
    {
        if(TryOpenChest()) return;
        var ore = _objectsRayCaster.TargetGathering;
        if(ore == null) return;
        ore.Gather(_inventory);
    }
}