using Firebase.Database;
using UnityEngine;

public class FirebaseSetup : MonoBehaviour
{
    public static FirebaseSetup singleton { get; set; }

    public DatabaseReference DatabaseReference { get; set; }

#if !UNITY_SERVER
    private void Awake()
    {
        if(singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        DatabaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        DontDestroyOnLoad(this);
    }
#endif

    [ContextMenu("Check Connection")]
    private async void CheckConnection()
    {
        await DatabaseReference.Child("Test").SetValueAsync("Done");
        Debug.Log("Test connection send");
    }
}
