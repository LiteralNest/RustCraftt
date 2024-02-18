
using ParrelSync;
using UnityEngine;

namespace Cloud.DataBaseSystem.UserData
{
    public class UserDataHandler : MonoBehaviour
    {
        public static UserDataHandler Singleton { get; set; }
        [field: SerializeField] public UserData UserData { get; set; }

        private void Awake()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
            Singleton = this;
            
            #if UNITY_EDITOR
            if (ClonesManager.IsClone())
            {
                UserData = new UserData(1001, "Clone");
            }
            #endif
        }
    }
}