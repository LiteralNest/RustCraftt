using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class TerrainSettings : MonoBehaviour
    {

        public List<GameObject> vegetationObjects; 

        private void Start()
        {
            UpdateGrassVisibility(GlobalValues.EnableGrass);
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


