using System.Collections.Generic;
using UnityEngine;

public class ItemsContainer : MonoBehaviour
{
    public static ItemsContainer singleton { get; set; }

    [field: SerializeField] public List<Item> Items { get; set; }

    private void Awake()
        => singleton = this;

    public Item GetItemById(int id)
    {
        foreach (var item in Items)
        {
            if (item.Id == id)
                return item;
        }
        Debug.LogError("Can't find item with id: " + id);
        return null;
    }
}