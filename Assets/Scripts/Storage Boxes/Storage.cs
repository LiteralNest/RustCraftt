using System.Collections.Generic;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Storage_Boxes
{
    public abstract class Storage : NetworkBehaviour
    {
        public NetworkList<Vector3> ItemsNetData { get; set; }

        [FormerlySerializedAs("_cells")] [SerializeField]
        public List<InventoryCell> Cells;

        public SlotsDisplayer SlotsDisplayer { get; set; }

        [Header("Test")] [SerializeField] private InventoryCell _testAddingCell;

        private void Awake()
            => ItemsNetData = new NetworkList<Vector3>();

        private void Start()
        {
            gameObject.tag = "LootBox";
        }

        #region virtual

        public virtual void InitBox()
            => SaveNetData();

        public virtual void CheckCells()
        {
        }

        public virtual void Open(InventoryHandler handler)
        {
            Appear();
            SlotsDisplayer.AssignStorage(this);
            SlotsDisplayer.InitCells();
            SlotsDisplayer.DisplayCells();
        }

        protected virtual void Appear()
            => ActiveInvetoriesHandler.singleton.AddActiveInventory(this);

        #endregion

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                InitBox();
            else
                ConvertWebData();
            // _itemsNetData.OnListChanged += ConvertWebData;
            base.OnNetworkSpawn();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResetItemServerRpc(int id, bool shouldConvertData = true)
        {
            if (IsServer)
                ItemsNetData[id] = new Vector3Int(-1, 0, -1);
            if(shouldConvertData)
                ConvertWebData();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResetItemsServerRpc()
        {
            if (!IsServer) return;
            for (int i = 0; i < ItemsNetData.Count; i++)
                ItemsNetData[i] = new Vector3Int(-1, 0, -1);
            ConvertWebData();
        }

        [ServerRpc(RequireOwnership = false)]
        private void ClearListServerRpc()
        {
            if (IsServer)
                ItemsNetData.Clear();
        }
        
        public void AssignCells(NetworkList<Vector3> inputList)
        {
            ItemsNetData = inputList;
            ConvertWebData();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetItemServerRpc(int cellId, int itemId, int count, int hp = -1, bool shouldSaveNetData = true,
            bool shouldDisplay = true)
        {
            if (IsServer)
                ItemsNetData[cellId] = new Vector3Int(itemId, count, hp);
            if (shouldSaveNetData)
                ConvertWebData(shouldDisplay);
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddItemCountServerRpc(int cellId, int itemId, int count)
        {
            if (IsServer)
            {
                ItemsNetData[cellId] = new Vector3Int(itemId, (int)ItemsNetData[cellId].y);
                ItemsNetData[cellId] += new Vector3Int(0, count);
            }

            ConvertWebData();
        }

        [ServerRpc(RequireOwnership = false)]
        public void RemoveItemCountServerRpc(int itemId, int count)
        {
            if (IsServer)
            {
                for (int i = 0; i < ItemsNetData.Count; i++)
                {
                    if (ItemsNetData[i].x == itemId)
                        ItemsNetData[i] -= new Vector3Int(0, count, 0);
                    if (ItemsNetData[i].y <= 0)
                        ItemsNetData[i] = new Vector3Int(-1, 0, -1);
                    
                }
            }

            ConvertWebData();
        }

        public void RemoveItems(List<InventoryCell> cells)
        {
            foreach (var cell in cells)
                RemoveItemCountServerRpc(cell.Item.Id, cell.Count);
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddItemToDesiredSlotServerRpc(int itemId, int count)
        {
            if (IsServer)
            {
                InventoryHelper.AddItemToDesiredSlot(itemId, count, Cells);
                SaveNetData();
            }

            ConvertWebData();
        }

        protected void SaveNetData()
            => CustomDataSerializer.SetConvertedItemsList(Cells, ItemsNetData);

        public bool EnoughMaterials(List<InventoryCell> inputCells)
            => InventoryHelper.EnoughMaterials(inputCells, Cells);

        public int GetItemCount(int id)
            => InventoryHelper.GetItemCount(id, Cells);

        private void ConvertWebData(bool shouldDisplayeCells = true)
        {
            Cells = CustomDataSerializer.GetConvertedCellsList(ItemsNetData);
            CheckCells();
            if (SlotsDisplayer == null) return;
            if (shouldDisplayeCells)
                SlotsDisplayer.DisplayCells();
        }

        public virtual bool CanAddItem(Item item)
            => true;

        protected void RemoveItem(Item item, int count)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                if (Cells[i].Item == item)
                    RemoveItemCountServerRpc(Cells[i].Item.Id, count);
            }
        }

        [ContextMenu("DebugNetCells")]
        private void DebugNetCells()
        {
            foreach (var cell in ItemsNetData)
                Debug.Log("Id: " + cell.x + " Count: " + cell.y + " HP: " + cell.z);
        }

        [ContextMenu("Test")]
        private void AddTestCell()
            => AddItemToDesiredSlotServerRpc(_testAddingCell.Item.Id, _testAddingCell.Count);
    }
}