using UnityEngine;

namespace Web.User
{
    public class UserDataHandler : MonoBehaviour
    {
        public static UserDataHandler singleton { get; set; }
        [field: SerializeField] public UserData UserData { get; set; }

#if !UNITY_SERVER
    
        private void Awake()
        {
            if (singleton != null && singleton != this)
            {
                Destroy(gameObject);
                return;
            }
            singleton = this;
        }

#endif
    }
}