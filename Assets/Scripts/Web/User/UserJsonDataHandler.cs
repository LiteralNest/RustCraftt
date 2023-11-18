using System.IO;
using UnityEngine;

public class UserJsonDataHandler : MonoBehaviour
{
    [Header("Start init")]
    [SerializeField] private string _fileName = "UserData";

    [Header("In game init")]
    [SerializeField] private string _fullJsonPath;

    private void Start()
    {
#if !UNITY_SERVER

        InitPath();
        LoadUserData(out bool exists, out UserData data);
#endif
    }

    private void InitPath()
    {
        _fullJsonPath = Path.Combine(Application.persistentDataPath, _fileName);
    }

    private void LoadUserData(out bool exists, out UserData data)
    {
        exists = false;
        data = default;
        if (!File.Exists(_fullJsonPath)) return;
        string json = File.ReadAllText(_fullJsonPath);
        data = JsonUtility.FromJson<UserData>(json);
        exists = true;
    }

    public void SaveUserData(string json)
        => File.WriteAllText(_fullJsonPath, json);
}