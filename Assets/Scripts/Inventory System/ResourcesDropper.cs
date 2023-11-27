using Inventory_System.Inventory_Items_Displayer;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourcesDropper : MonoBehaviour, IDropHandler
{
    public static ResourcesDropper singleton { get; private set; }

    public Inventory_System.Inventory_Items_Displayer.InventoryItemDisplayer InventoryItemDisplayer { get; set; }

    [SerializeField] private float _droppingOffset = 1;
    
    private void Start()
        => singleton = this;


    private Vector3 GetFrontCameraPos()
    {
        var camera = Camera.main;
        Vector3 cameraPosition = camera.transform.position;
        Vector3 cameraForward = camera.transform.forward;
        return cameraPosition + (cameraForward * _droppingOffset);
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (!InventoryItemDisplayer) return;
        InventoryHandler.singleton.CharacterInventory.RemoveItem(InventoryItemDisplayer.InventoryCell.Item.Id, InventoryItemDisplayer.InventoryCell.Count);
        InstantiatingItemsPool.sigleton.SpawnDropableObjectServerRpc(InventoryItemDisplayer.InventoryCell.Item.Id, InventoryItemDisplayer.InventoryCell.Count, GetFrontCameraPos());
        Destroy(InventoryItemDisplayer.gameObject);
    }
}
