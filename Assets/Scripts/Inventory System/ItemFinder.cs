using System.Collections;
using System.Collections.Generic;
using Items_System.Items.Abstract;
using UnityEngine;

public class ItemFinder : MonoBehaviour
{
    public static ItemFinder singleton {get; private set;}

    private void Awake()
        => singleton = this; 
    
    [SerializeField] private List<Item> _items;

    public Item GetItemById(int id)
    {
        foreach (var item in _items)
        {
            if (item.Id == id)
                return item;
        }

        Debug.LogError("Can't find item with id: " + id);
        return null;
    }

    public List<CraftingItem> GetCraftingItems()
    {
        var res = new List<CraftingItem>();
        foreach (var item in _items)
            if (item is CraftingItem)
                res.Add(item as CraftingItem);
        return res;
    }
}
