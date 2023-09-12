using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class FirebaseInventoryDataSender : MonoBehaviour
{
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
            cells.Add(new SendingDataField{Count = cell.Count, ItemId = cell.Item.Id});
        }
        data.Cells = cells;
        return data;
    }
    
    private void SaveData(List<InventoryCell> cells)
    {
        SendingData data = new SendingData();
        string json = JsonUtility.ToJson(GetConvertedSendingData(cells));
        FirebaseSetup.singleton.DatabaseReference.Child(UserDataHandler.singleton.UserData.Name).Child("Server1").Child("Inventory").SetRawJsonValueAsync(json);
    }
}
