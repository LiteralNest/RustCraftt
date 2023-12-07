using System.Threading.Tasks;
using UnityEngine;
using Web.User;

[RequireComponent(typeof(ObjectsRayCaster))]
public class PlayerResourcesGatherer : MonoBehaviour
{
    [Header("Attached Scripts")]
    [SerializeField] private InventoryHandler _inventoryHandler;
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;
    [SerializeField] private AudioSource _audioSource;
    private float _recoveringTime = 1; 

    [Header("Ore")]
    [SerializeField] private bool _canHit = true;
    
    [Header("Gathering")]
    public bool _gathering = false;
    public bool StartedGather { get; private set; }

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
        if(!ResourceGatheringObject) return;
        _recoveringTime = ResourceGatheringObject.GatheringAnimation.length;
        _canHit = false;
        await Task.Delay((int)(_recoveringTime * 1000));
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
        TryOpenWorkBench();
        TryHit();
        TryOpenChest();
        TryGather();
        TryOpenCampFire();
        TryOpenRecycler();
        TryPickUp();
        TryOpenDoor();
    }

    private void TryOpenWorkBench()
    {
        var bench = _objectsRayCaster.WorkBench;
        if (!bench) return;
        bench.Open();
    }
    
    private void TryOpenDoor()
    {
        var door = _objectsRayCaster.DoorHandler;
        if (!door) return;
        door.Open(UserDataHandler.singleton.UserData.Id);
    }
    
    private void TryHit()
    {
        if (!_canHit) return;
        Recover();
        var ore = _objectsRayCaster.TargetResourceOre;
        if (!ore) return;
        if(InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.GetCurrentHp() <= 0) return;
        StartedGather = true;
        ore.MinusHp(_inventoryHandler.ActiveItem, out bool destroyed, _objectsRayCaster.LastRaycastedPosition, _objectsRayCaster.LastRayCastedRotation);
        _audioSource.PlayOneShot(ore.GatheringClip);
        if (!destroyed) return;
        StopGathering();
    }

    private bool TryAuthorizeClipboard()
    {
        var clipboard = _objectsRayCaster.ToolClipboard;
        if(!clipboard) return false;
        clipboard.AuthorizeServerRpc(UserDataHandler.singleton.UserData.Id);
        return true;
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
        if(TryAuthorizeClipboard()) return;
        if(TryOpenChest()) return;
        var ore = _objectsRayCaster.TargetGathering;
        if(!ore) return;
        ore.Gather();
    }

    public void TryOpenCampFire()
    {
        var campfire = _objectsRayCaster.Smelter;
        if(!campfire) return;
        campfire.Open(_inventoryHandler);
    }

    public void TryOpenRecycler()
    {
        var recylcer = _objectsRayCaster.RecyclerHandler;
        if(!recylcer) return;
        recylcer.Open(_inventoryHandler);
    }

    public void TryPickUp()
    {
        var lootingItem = _objectsRayCaster.LootingItem;
        if(!lootingItem) return;
        _inventoryHandler.CharacterInventory.AddItemToDesiredSlotServerRpc(lootingItem.ItemId.Value,
            lootingItem.Count.Value, 0);
        lootingItem.PickUpServerRpc();
    }
}