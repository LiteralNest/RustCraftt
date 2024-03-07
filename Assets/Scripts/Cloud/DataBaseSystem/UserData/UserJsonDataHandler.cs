using System.IO;
using System.Threading.Tasks;
using Cloud.DataBaseSystem.DataBaseServices;
using Cloud.DataBaseSystem.UserData.View;
using UnityEngine;

namespace Cloud.DataBaseSystem.UserData
{
    public class UserJsonDataHandler : MonoBehaviour
    {
        [Header("Start init")]
        [SerializeField] private RegisterPlayerView _registerPlayerView;
        [SerializeField] private string _fileName = "UserData.rc";

        private DataBaseUserGetter _dataBaseUserGetter = new();
        private string _fullJsonPath;

        private async void Start()
        {
#if !UNITY_SERVER
            InitPath();
            if (!TryLoadUserData(out UserData data))
            {
                _registerPlayerView.Init();
                return;
            }
            await SaveUserData(data.Name);
#endif
        }

        private string DeleteFirstAndLastCharacter(string inputString)
        {
            if (inputString.Length >= 2)
                return inputString.Substring(1, inputString.Length - 2);
            return string.Empty;
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

        public async Task SaveUserData(string userName)
        {
            _registerPlayerView.HandleInputFields(false);
            var res = await _dataBaseUserGetter.PlayerExistsAsync(userName);
            if(res == "false")
                _registerPlayerView.Init();
            else
            {
                _registerPlayerView.DisplayRegisterPanel(false);
                var fixedRes = DeleteFirstAndLastCharacter(res);
                var data = JsonUtility.FromJson<UserData>(fixedRes);
                UserDataHandler.Singleton.UserData = data;
                await File.WriteAllTextAsync(_fullJsonPath, fixedRes);
            }
        }
    }
}