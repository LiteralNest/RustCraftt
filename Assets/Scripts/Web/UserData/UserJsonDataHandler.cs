using System.IO;
using UnityEngine;
using Web.UserData.View;

namespace Web.UserData
{
    public class UserJsonDataHandler : MonoBehaviour
    {
        [Header("Start init")] 
        [SerializeField] private RegisterPlayerView _registerPlayerView;
        [SerializeField] private string _fileName = "UserData.rc";

        private string _fullJsonPath;

        private void Start()
        {
#if !UNITY_SERVER

            InitPath();
            if (!TryLoadUserData(out UserData data))
            {
                _registerPlayerView.Init(this);
                return;
            }

            UserDataHandler.Singleton.UserData = data;
#endif
        }

        private void InitPath()
            => _fullJsonPath = Path.Combine(Application.persistentDataPath, _fileName);
        
        private bool TryLoadUserData(out UserData data)
        {
            data = default;
            if (!File.Exists(_fullJsonPath)) return false;
            string json = File.ReadAllText(_fullJsonPath);
            data = JsonUtility.FromJson<UserData>(json);
            return true;
        }

        public void SaveUserData(string name)
        {
            int id = Random.Range(0, 100000);
            UserData data = new UserData(id, name);
            File.WriteAllText(_fullJsonPath, JsonUtility.ToJson(data));
            UserDataHandler.Singleton.UserData = data;
        }
    }
}