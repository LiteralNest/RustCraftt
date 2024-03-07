using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class TerrainSettingsCatcher : MonoBehaviour
    {

        public List<GameObject> vegetationObjects; 

        private void Start()
        {
            var settings = SettingsContainer.Singleton;
            if(settings == null) return;
            UpdateGrassVisibility(settings.EnableGrass);
        }


        private void UpdateGrassVisibility(bool enableGrass)
        {
            foreach (var grassObject in vegetationObjects)
            {
                grassObject.SetActive(enableGrass);
            }
        }
    }
}


