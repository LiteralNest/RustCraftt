using Storage_System;
using UnityEngine;

namespace Inventory_System
{
    public class CurrentInventoriesHandler : MonoBehaviour
    {
        public static CurrentInventoriesHandler Singleton { get; set; }

        [field: SerializeField] public Storage CurrentStorage { get; set; }

        private void Awake()
            => Singleton = this;

        public void ResetCurrentStorage()
            => CurrentStorage = null;

        public void HandleCurrentStoragePanel(bool value)
        {
            if(CurrentStorage == null) return;
            CurrentStorage.HandleUi(value);
        }
    }
}