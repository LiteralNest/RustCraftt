using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ObjectsRayCaster))]
public class PlayerResourcesGatherer : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private InventoryHandler _inventoryHandler;
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;
    
    [Header("Ore")]
    [SerializeField] private float _recoveringSpeed = 0.3f;
    [SerializeField] private bool _canHit = true;
    
    [Header("Gathering")]
    [SerializeField] private float _maxOreHitDistance = 5f;
    private bool _gathering = false;
    
    public ResourceGatheringObject ResourceGatheringObject { get; private set; }

    private void OnEnable()
        => GlobalEventsContainer.ResourceGatheringObjectAssign += AssignResourceGatheringObject;
    
    private void OnDisable()
        => GlobalEventsContainer.ResourceGatheringObjectAssign -= AssignResourceGatheringObject;
    
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

    private void AssignResourceGatheringObject(ResourceGatheringObject target)
    {
        _objectsRayCaster.CanRayCastOre = target != null;
        ResourceGatheringObject = target;
    }

    private async void Recover()
    {
        _canHit = false;
        await Task.Delay((int)(_recoveringSpeed * 1000));
        _canHit = true;
    }

    public void StartGathering()
    {
        ResourceGatheringObject.SetGathering(true);
        _gathering = true;
    }
    
    public void StopGathering()
    {
        ResourceGatheringObject.SetGathering(false);
        _gathering = false;
    }

    public void TryDoGathering()
    {
        TryHit();
        TryOpenChest();
        TryGather();
        TryOpenCampFire();
    }
    
    private void TryHit()
    {
        if (!_canHit) return;
        Recover();
        var ore = _objectsRayCaster.TargetResourceOre;
        if (!ore) return;
        ore.MinusHp(_inventoryHandler.ActiveItem, out bool destroyed);
        if (!destroyed) return;
        StopGathering();
    }

    private bool TryOpenChest()
    {
        var chest = _objectsRayCaster.TargetBox;
        if(!chest) return false;
        chest.Open(_inventoryHandler);
        return true;
    }
    
    public void TryGather()
    {
        if(TryOpenChest()) return;
        var ore = _objectsRayCaster.TargetGathering;
        if(!ore) return;
        ore.Gather();
    }

    public void TryOpenCampFire()
    {
        var campfire = _objectsRayCaster.CampFireHandler;
        if(!campfire) return;
        campfire.Open(_inventoryHandler);
    }
}