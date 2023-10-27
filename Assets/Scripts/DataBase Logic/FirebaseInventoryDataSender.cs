using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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

    private void SaveData(List<InventoryCell> cells)
    {
        string json = JsonUtility.ToJson(WebDataConverter.GetConvertedSendingData(cells));
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