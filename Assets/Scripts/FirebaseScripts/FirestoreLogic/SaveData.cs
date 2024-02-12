using Firebase.Firestore;

[FirestoreData]
public class SaveData
{
    private string _id = "123";
    private string _userName = "John Wick";
    private int _health = 100;
    private float _money = 1000f;

    [FirestoreProperty]
    public string Username
    {
        get => _userName;
        set => _userName = value;
    }
    
    [FirestoreProperty]
    public string ID
    {
        get => _id;
        set => _id = value;
    }
    
    [FirestoreProperty]
    public int Health
    {
        get => _health;
        set => _health = value;
    }
    
    [FirestoreProperty]
    public float Money
    {
        get => _money;
        set => _money = value;
    }
}