using UnityEngine;

public class UserDataHandler : MonoBehaviour
{
    public static UserDataHandler singleton { get; set; }

    [field: SerializeField] public UserData UserData { get; set; }

    private void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
    }
}