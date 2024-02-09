using UnityEngine;

namespace Building_System.Upgrading
{
    [System.Serializable]
    public struct BuildingUpgradeSlot
    {
        [SerializeField] private int _level;
        [SerializeField] private string _upgradeName;
        
        public int Level => _level;
        public string UpgradeName => _upgradeName;
    }
}