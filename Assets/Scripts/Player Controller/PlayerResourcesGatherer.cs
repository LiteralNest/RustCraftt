using System.Threading.Tasks;
using Events;
using InHandItems;
using Items_System.Items;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

[RequireComponent(typeof(ObjectsRayCaster))]
public class PlayerResourcesGatherer : MonoBehaviour
{
    [Header("Attached Scripts")] [SerializeField]
    private PlayerNetCode _playerNetCode;

    [SerializeField] private InventoryHandler _inventoryHandler;
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;
    private float _recoveringTime = 1;

    public ResourceGatheringObject ResourceGatheringObject { get; private set; }

    private void Start()
    {
        if (_objectsRayCaster == null)
            _objectsRayCaster = GetComponent<ObjectsRayCaster>();
    }

    public void TryDoGathering()
    {
        TryTurnOnSatchel();
        TryHelp();
        TryRenameSleepingBag();
        TrySit();
        TryOpenWorkBench();
        TryOpenChest();
        TryGather();
        TryOpenCampFire();
        TryOpenRecycler();
        TryPickUp();
        TryOpenDoor();
    }

    private void TryTurnOnSatchel()
    {
        var satchel = _objectsRayCaster.SatchelExplosive;
        if (!satchel) return;
        satchel.TurnOn();
    }

    private void TryRenameSleepingBag()
    {
        var sleepingBag = _objectsRayCaster.TargetSleepingBag;
        if (!sleepingBag) return;
        sleepingBag.Open();
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
        door.Open(UserDataHandler.Singleton.UserData.Id);
    }

    private bool TrySit()
    {
        var place = _objectsRayCaster.TargetSittingPlace;
        if (!place) return false;
        if (place.CanSit())
            place.SitIn(_playerNetCode);
        else if (place.CanStand(_playerNetCode))
            place.StandUp(_playerNetCode);
        return true;
    }

    private bool TryAuthorizeClipboard()
    {
        var clipboard = _objectsRayCaster.ToolClipboard;
        if (!clipboard) return false;
        clipboard.AuthorizeServerRpc(UserDataHandler.Singleton.UserData.Id);
        return true;
    }

    private bool TryOpenChest()
    {
        var chest = _objectsRayCaster.TargetBox;
        if (!chest) return false;
        chest.Open(_inventoryHandler);
        return true;
    }

    private void TryGather()
    {
        if (TryAuthorizeClipboard()) return;
        if (TryOpenChest()) return;
        var ore = _objectsRayCaster.TargetGathering;
        if (!ore) return;
        ore.Gather();
    }

    private void TryOpenCampFire()
    {
        var campfire = _objectsRayCaster.Smelter;
        if (!campfire) return;
        campfire.Open(_inventoryHandler);
    }

    private void TryOpenRecycler()
    {
        var recylcer = _objectsRayCaster.RecyclerHandler;
        if (!recylcer) return;
        recylcer.Open(_inventoryHandler);
    }

    private void TryPickUp()
    {
        var lootingItem = _objectsRayCaster.LootingItem;
        if (!lootingItem) return;
        _inventoryHandler.CharacterInventory.AddItemToDesiredSlotServerRpc(lootingItem.Data.Id,
            lootingItem.Data.Count, lootingItem.Data.Ammo, lootingItem.Data.Hp);
        lootingItem.PickUpServerRpc();
    }

    private void TryHelp()
    {
        var knockDowner = _objectsRayCaster.PlayerKnockDowner;
        if (!knockDowner) return;
        knockDowner.StandUpServerRpc();
    }
}