using UnityEngine;

public class ServerData : MonoBehaviour
{
    public static ServerData singleton { get; set; }

    public int ServerId { get; set; } = 7777;
    
    private void Awake()
    {
        if (singleton != null && singleton != this)
            Destroy(gameObject);
        singleton = this;
        DontDestroyOnLoad(gameObject);
    }
}