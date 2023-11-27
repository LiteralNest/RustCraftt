using System.Collections;
using System.Collections.Generic;
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
        {
            ItemsNetData = new NetworkList<Vector3>(new List<Vector3>(), NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Server);
        }

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

        protected virtual void DoAfterRemovingItem(InventoryCell cell)
        {
        }

        protected virtual void DoAfterAddingItem(InventoryCell cell)
        {
        }

        #endregion

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
                InitBox();
            else
                ConvertWebData();
            // ItemsNetData.OnListChanged += Test;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ResetItemServerRpc(int id, bool shouldConvertData = true)
        {
            if (IsServer)
                ItemsNetData[id] = new Vector3Int(-1, 0, -1);
            if (shouldConvertData)
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

        public void AddItem(int cellId, int itemId, int count)
        {
            AddItemCountServerRpc(cellId, itemId, count);
            DoAfterAddingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddItemCountServerRpc(int cellId, int itemId, int count)
        {
            ItemsNetData[cellId] = new Vector3Int(itemId, (int)ItemsNetData[cellId].y);
            ItemsNetData[cellId] += new Vector3Int(0, count);

            ConvertWebData();
        }

        public void RemoveItem(int itemId, int count)
        {
            RemoveItemCountServerRpc(itemId, count);
            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        private void RemoveItemCountServerRpc(int itemId, int count)
        {
            for (int i = 0; i < ItemsNetData.Count; i++)
            {
                if (ItemsNetData[i].x == itemId)
                    ItemsNetData[i] -= new Vector3Int(0, count, 0);
                if (ItemsNetData[i].y <= 0)
                    ItemsNetData[i] = new Vector3Int(-1, 0, -1);
            }

            DoAfterRemovingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));

            ConvertWebData();
        }

        public void RemoveItems(List<InventoryCell> cells)
        {
            foreach (var cell in cells)
                RemoveItemCountServerRpc(cell.Item.Id, cell.Count);
        }

        private IEnumerator AddItemToServerWithRoutine(int itemId, int count)
        {
            yield return new WaitForSeconds(0.2f);
            AddItemToDesiredSlotServerRpc((ushort)itemId, (ushort)count);
        }

        public void AddCraftedItem(int itemId, int count)
        {
            StartCoroutine(AddItemToServerWithRoutine(itemId, count));
            DoAfterAddingItem(new InventoryCell(ItemFinder.singleton.GetItemById(itemId), count));
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddItemToDesiredSlotServerRpc(ushort itemId, ushort count)
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
            => AddItemToDesiredSlotServerRpc((ushort)_testAddingCell.Item.Id, (ushort)_testAddingCell.Count);
    }
}