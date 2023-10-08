using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CampFireHandler : NetworkBehaviour
{
    [Header("Web")] [SerializeField] private NetworkVariable<bool> _flaming = new(false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    [Header("Main Params")]
    [SerializeField] private GameObject _fireObject;
    
    [field:SerializeField] public List<Item> AvaliableFuel { get; private set; }
    [field:SerializeField] public List<CookingFood> AvaliableFoodForCooking { get; private set; }
     
    [field:SerializeField] public List<InventoryCell> Cells { get; private set; }

    private CampFireSlotsContainer _targetSlotsContainer;

    private bool _fireTurned;
    
    private void Start()
    {
        TurnFire();
        _flaming.OnValueChanged += (bool prevValue, bool newValue) =>
        {
            TurnFire();
        };
    }
    
    public void SetItem(int index, InventoryCell cell)
    {
        Cells[index].Item = cell.Item;
        Cells[index].Count = cell.Count;
    }

    private void SetItemCount(int index, int count)
    {
        Cells[index].Count = count;
    }

    public void Open(InventoryHandler handler)
    {
        handler.OpenCampFirePanel();
        _targetSlotsContainer = handler.CampFireSlotsContainer;
        _targetSlotsContainer.Init(this);
    }

    private void TurnFire()
        => _fireObject.SetActive(_flaming.Value);

    [ServerRpc(RequireOwnership = false)]
    public void TurnFlamingServerRpc(bool value)
    {
        _fireTurned = value;
        _flaming.Value = value;
    }
}