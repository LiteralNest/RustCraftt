using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class LootingItem : MonoBehaviour
{
    public Item Item => _item;
    [SerializeField] private Item _item;
    public int Count { get; set; }

    private void Start()
        => gameObject.tag = "LootingItem";
    
    
}