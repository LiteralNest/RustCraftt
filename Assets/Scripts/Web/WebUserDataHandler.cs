using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class WebUserDataHandler : MonoBehaviour
{
    public static WebUserDataHandler singleton { get; private set; }
    
    private DatabaseReference _databaseReference;
    private void Awake()
    {
        singleton = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
        => _databaseReference = FirebaseSetup.singleton.DatabaseReference;
    
    public async Task<string> GetUserValueById(int id)
    {
        var task = _databaseReference.Child("Users").Child(id.ToString()).Child("Name").GetValueAsync();
        await task;
        return task.Result.Value.ToString();
    }
}
