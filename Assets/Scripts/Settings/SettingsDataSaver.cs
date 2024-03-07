using System.IO;
using UnityEngine;

namespace Settings
{
    public class SettingsDataSaver
    {
        private string GetPath()
            => Application.persistentDataPath + "/Settings.json";

        public void SaveData(SettingsData data)
        {
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(GetPath(), json);
        }

        public SettingsData LoadData()
        {
            var path = GetPath();
            if (!File.Exists(path))
                return null;
            var json = File.ReadAllText(GetPath());
            return JsonUtility.FromJson<SettingsData>(json);
        }
    }
}