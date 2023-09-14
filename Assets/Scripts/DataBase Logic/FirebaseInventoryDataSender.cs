using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public struct SendingDataField
{
    public int Count;
    public int ItemId;
}

[System.Serializable]
public struct SendingData
{
    public List<SendingDataField> Cells;
}

public class FirebaseInventoryDataSender : MonoBehaviour
{
    public static FirebaseInventoryDataSender singleton { get; set; }

    private void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(gameObject);
        singleton = this;
    }
    
    private void OnEnable()
        => GlobalEventsContainer.InventoryDataShouldBeSaved += SaveData;

    private void OnDisable()
        => GlobalEventsContainer.InventoryDataShouldBeSaved -= SaveData;

    private SendingData GetConvertedSendingData(List<InventoryCell> inputCells)
    {
        SendingData data = new SendingData();
        List<SendingDataField> cells = new List<SendingDataField>();
        foreach (var cell in inputCells)
        {
            if (cell.Item == null) continue;
            cells.Add(new SendingDataField { Count = cell.Count, ItemId = cell.Item.Id });
        }

        data.Cells = cells;
        return data;
    }

    private void SaveData(List<InventoryCell> cells)
    {
        SendingData data = new SendingData();
        string json = JsonUtility.ToJson(GetConvertedSendingData(cells));
        FirebaseSetup.singleton.DatabaseReference
            .Child("Servers")
            .Child(ServerData.singleton.ServerId.ToString())
            .Child(UserDataHandler.singleton.UserData.Name)
            .Child("Inventory").SetRawJsonValueAsync(json);
    }

    [ContextMenu("Test")]
    public async Task<List<SendingDataField>> TryLoadData()
    {
        var request = FirebaseSetup.singleton.DatabaseReference
            .Child("Servers")
            .Child(ServerData.singleton.ServerId.ToString())
            .Child(UserDataHandler.singleton.UserData.Name)
            .Child("Inventory")
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

        List<SendingDataField> cells = new List<SendingDataField>();
        foreach (var item in request.Result.Children)
        {
            int count = Int32.Parse(item.Child("Count").Value.ToString());
            int itemId = Int32.Parse(item.Child("ItemId").Value.ToString());
            cells.Add(new SendingDataField() { Count = count, ItemId = itemId });
        }

        return cells;
    }
}