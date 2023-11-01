using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class WebServerDataHandler : MonoBehaviour
{
    public static WebServerDataHandler singleton { get; private set; }

    [field: SerializeField] public string ServerId { get; private set; } = "127001";
    [SerializeField] private string _campFiresPath = "CampFires";
    [SerializeField] private string _lootBoxPath = "LootBoxes";
    [SerializeField] private string _storageBoxPath = "StorageBoxes";
    [SerializeField] private string _usersPath = "Users";

    private DatabaseReference _dbReference;

    private void OnEnable()
    {
        GlobalEventsContainer.InventoryDataShouldBeSaved += SaveInventoryData;
        GlobalEventsContainer.CampFireDataShouldBeSaved += SaveCampFireData;
        GlobalEventsContainer.LootBoxDataShouldBeSaved += SaveLootBoxData;
    }

    private void OnDisable()
    {
        GlobalEventsContainer.InventoryDataShouldBeSaved -= SaveInventoryData;
        GlobalEventsContainer.CampFireDataShouldBeSaved -= SaveCampFireData;
        GlobalEventsContainer.LootBoxDataShouldBeSaved -= SaveLootBoxData;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        singleton = this;
    }

    private void Start()
        => _dbReference = FirebaseDatabase.DefaultInstance.RootReference;

    #region User

    private async Task<bool> UserExistsOnServer(int id)
    {
        var task = await _dbReference
            .Child(ServerId)
            .Child(_usersPath)
            .Child(id.ToString())
            .GetValueAsync();
        return task.Exists;
    }

    public async void RegistrateNewUser()
    {
        if (await UserExistsOnServer(UserDataHandler.singleton.UserData.Id)) return;
        var InventoryCells = new List<InventoryCell>();
        var sendingData = WebDataConverter.GetConvertedSendingData(InventoryCells);
        UserServerData data = new UserServerData();
        data.Id = UserDataHandler.singleton.UserData.Id;
        data.InventorySendingData = sendingData;
        await _dbReference
            .Child(ServerId)
            .Child(_usersPath)
            .Child(data.Id.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(data));
    }

    #endregion

    #region Inventory

    private void SaveInventoryData(List<InventoryCell> cells)
    {
        InventorySendingData data = WebDataConverter.GetConvertedSendingData(cells);

        _dbReference
            .Child(ServerId)
            .Child(_usersPath)
            .Child(UserDataHandler.singleton.UserData.Id.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(data));
    }

    public async Task<List<InventorySendingDataField>> LoadInventoryData()
    {
        var request = FirebaseSetup.singleton.DatabaseReference
            .Child(ServerId)
            .Child(_usersPath)
            .Child(UserDataHandler.singleton.UserData.Id.ToString())
            .Child("Cells")
            .GetValueAsync();
        await request;

        if (request.Exception != null)
        {
            Debug.LogError(message: $"Failed to read value: {request.Exception.Message}");
            return null;
        }

        if (request.Result.Value == null)
            return null;

        List<InventorySendingDataField> cells = new List<InventorySendingDataField>();
        foreach (var item in request.Result.Children)
        {
            int count = Int32.Parse(item.Child("Count").Value.ToString());
            int itemId = Int32.Parse(item.Child("ItemId").Value.ToString());
            cells.Add(new InventorySendingDataField() { Count = count, ItemId = itemId });
        }

        return cells;
    }

    #endregion

    #region CampFire

    private async Task<int> GetLastCampFireId()
    {
        var task = await _dbReference
            .Child(ServerId)
            .Child(_campFiresPath)
            .OrderByChild("Id")
            .LimitToLast(1)
            .GetValueAsync();
        if (!task.Exists || task.ChildrenCount == 0) return 0;
        foreach (var child in task.Children)
        {
            CampFireData data = JsonUtility.FromJson<CampFireData>(child.GetRawJsonValue());
            return data.Id;
        }

        return 0;
    }

    public async Task<int> RegistrateNewCampFire()
    {
        int id = await GetLastCampFireId();
        var InventoryCells = new List<InventoryCell>();
        var sendingData = WebDataConverter.GetConvertedSendingData(InventoryCells);
        CampFireData data = new CampFireData();
        data.Id = ++id;
        data.InventorySendingData = sendingData;
        await _dbReference
            .Child(ServerId)
            .Child(_campFiresPath)
            .Child(data.Id.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(data));
        return data.Id;
    }

    private void SaveCampFireData(List<InventoryCell> cells, int id)
    {
        CampFireData campFireData = new CampFireData();
        campFireData.Id = id;
        campFireData.InventorySendingData = WebDataConverter.GetConvertedSendingData(cells);
        _dbReference
            .Child(ServerId)
            .Child(_campFiresPath)
            .Child(id.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(campFireData));
    }

    public async Task<List<InventorySendingDataField>> LoadCampFireData(int id)
    {
        var request = FirebaseSetup.singleton.DatabaseReference
            .Child(ServerId)
            .Child(_campFiresPath)
            .Child(id.ToString())
            .Child("Cells")
            .GetValueAsync();
        await request;
        
        List<InventorySendingDataField> cells = new List<InventorySendingDataField>();
        foreach (var item in request.Result.Children)
        {
            int count = Int32.Parse(item.Child("Count").Value.ToString());
            int itemId = Int32.Parse(item.Child("ItemId").Value.ToString());
            cells.Add(new InventorySendingDataField() { Count = count, ItemId = itemId });
        }

        return cells;
    }

    #endregion

    #region LootBox

    private async Task<int> GetLastLootBox()
    {
        var task = await _dbReference
            .Child(ServerId)
            .Child(_lootBoxPath)
            .OrderByChild("Id")
            .LimitToLast(1)
            .GetValueAsync();
        if (!task.Exists || task.ChildrenCount == 0) return 0;
        foreach (var child in task.Children)
        {
            CampFireData data = JsonUtility.FromJson<CampFireData>(child.GetRawJsonValue());
            return data.Id;
        }

        return 0;
    }

    public async Task<int> RegistrateNewLootBox()
    {
        int id = await GetLastLootBox();
        var InventoryCells = new List<InventoryCell>();
        var sendingData = WebDataConverter.GetConvertedSendingData(InventoryCells);
        CampFireData data = new CampFireData();
        data.Id = ++id;
        data.InventorySendingData = sendingData;
        await _dbReference
            .Child(ServerId)
            .Child(_lootBoxPath)
            .Child(data.Id.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(data));
        return data.Id;
    }

    public void SaveLootBoxData(List<InventoryCell> cells, int id)
    {
        CampFireData data = new CampFireData();
        data.Id = id;
        data.InventorySendingData = WebDataConverter.GetConvertedSendingData(cells);
        _dbReference
            .Child(ServerId)
            .Child(_lootBoxPath)
            .Child(id.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(data));
    }

    public async Task<List<InventorySendingDataField>> LoadLootBoxData(int id)
    {
        var request = FirebaseSetup.singleton.DatabaseReference
            .Child(ServerId)
            .Child(_lootBoxPath)
            .Child(id.ToString())
            .Child("InventorySendingData")
            .Child("Cells")
            .GetValueAsync();
        await request;
        
        List<InventorySendingDataField> cells = new List<InventorySendingDataField>();
        foreach (var item in request.Result.Children)
        {
            int count = Int32.Parse(item.Child("Count").Value.ToString());
            int itemId = Int32.Parse(item.Child("ItemId").Value.ToString());
            cells.Add(new InventorySendingDataField() { Count = count, ItemId = itemId });
        }

        return cells;
    }

    #endregion

    #region StorageBox

    private async Task<int> GetLastStorageBox()
    {
        var task = await _dbReference
            .Child(ServerId)
            .Child(_storageBoxPath)
            .OrderByChild("Id")
            .LimitToLast(1)
            .GetValueAsync();
        if (!task.Exists || task.ChildrenCount == 0) return 0;
        foreach (var child in task.Children)
        {
            CampFireData data = JsonUtility.FromJson<CampFireData>(child.GetRawJsonValue());
            return data.Id;
        }

        return 0;
    }
    
    public async Task<int> RegistrateNewStorageBox()
    {
        int id = await GetLastStorageBox();
        var InventoryCells = new List<InventoryCell>();
        var sendingData = WebDataConverter.GetConvertedSendingData(InventoryCells);
        CampFireData data = new CampFireData();
        data.Id = ++id;
        data.InventorySendingData = sendingData;
        await _dbReference
            .Child(ServerId)
            .Child(_storageBoxPath)
            .Child(data.Id.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(data));
        return data.Id;
    }
    
    public void SaveStorageBoxData(List<InventoryCell> cells, int id)
    {
        CampFireData data = new CampFireData();
        data.Id = id;
        data.InventorySendingData = WebDataConverter.GetConvertedSendingData(cells);
        _dbReference
            .Child(ServerId)
            .Child(_storageBoxPath)
            .Child(id.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(data));
    }

    public async Task<List<InventorySendingDataField>> LoadStorageBoxData(int id)
    {
        var request = FirebaseSetup.singleton.DatabaseReference
            .Child(ServerId)
            .Child(_storageBoxPath)
            .Child(id.ToString())
            .Child("InventorySendingData")
            .Child("Cells")
            .GetValueAsync();
        await request;
        
        List<InventorySendingDataField> cells = new List<InventorySendingDataField>();
        foreach (var item in request.Result.Children)
        {
            int count = Int32.Parse(item.Child("Count").Value.ToString());
            int itemId = Int32.Parse(item.Child("ItemId").Value.ToString());
            cells.Add(new InventorySendingDataField() { Count = count, ItemId = itemId });
        }

        return cells;
    }
    
    #endregion
}