using UnityEngine;

namespace Web.User
{
    public class UserDataHandler : MonoBehaviour
    {
        public static UserDataHandler singleton { get; set; }
        [field: SerializeField] public UserData UserData { get; set; }

        private void Awake()
        {
            UserData.Id = Random.Range(0, 100000);
            
            if (singleton != null && singleton != this)
            {
                Destroy(gameObject);
                return;
            }
            singleton = this;
        }
    }
}