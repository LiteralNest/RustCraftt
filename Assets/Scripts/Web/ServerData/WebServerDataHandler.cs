using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class WebServerDataHandler : MonoBehaviour
{
    public static WebServerDataHandler singleton { get; private set; }
    
    [field: SerializeField] public string ServerId { get; private set; } = "127001";
    [SerializeField] private string _campFiresPath = "CampFires";

    private DatabaseReference _dbReference;

    [Header("Test")] [SerializeField] private Item _testItem;

    private void Awake()
        => singleton = this;
    
    private void Start()
        => _dbReference = FirebaseDatabase.DefaultInstance.RootReference;

    private async Task<int> GetLastCampFireId()
    {
        var task = await _dbReference.Child(ServerId).Child(_campFiresPath).OrderByChild("Id").LimitToLast(1).GetValueAsync();
        if(!task.Exists || task.ChildrenCount == 0) return 0;
        foreach (var child in task.Children)
        {
            CampFireData data = JsonUtility.FromJson<CampFireData>(child.GetRawJsonValue());
            return data.Id;
        }
        return 0;
    }

    [ContextMenu("Test Register")]
    public async Task RegistrateNewCampFire()
    {
        int id = await GetLastCampFireId();
        var InventoryCells = new List<InventoryCell>();
        InventoryCells.Add(new InventoryCell(_testItem, 2));
        var sendingData = WebDataConverter.GetConvertedSendingData(InventoryCells);
        CampFireData data = new CampFireData();
        data.Id = ++id;
        data.SendingData = sendingData;
        await _dbReference.Child(ServerId).Child(_campFiresPath).Child(data.Id.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(data));
    }
}