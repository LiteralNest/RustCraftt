using Firebase.Firestore;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private FirebaseFirestore _firestore;

    private void Awake()
    {
        _firestore = FirebaseFirestore.DefaultInstance;
    }

    public void SaveToCloud()
    {
        var saveData = new SaveData();
        _firestore.Document($"SaveData/0").SetAsync(saveData);
    }

    public void LoadToCloud()
    {
        
    }
}