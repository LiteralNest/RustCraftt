using System.Collections.Generic;
using System.Linq;
using Items_System.Items.Abstract;
using UnityEngine;

namespace Inventory_System
{
    public class ItemFinder : MonoBehaviour
    {
        private static readonly string Tag = "Main Resources";
        public static ItemFinder singleton {get; private set;}

        [SerializeField] private List<Item> _items;

        private void Awake()
        {
            singleton = this;
            _items = Resources.LoadAll<Item>(Tag).ToList();
        }

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
            {
                if(!(item is CraftingItem)) continue;
                var craftItem = item as CraftingItem;
                if (GlobalValues.AdministratorBuild)
                {
                    res.Add(item as CraftingItem);
                    continue;
                }
                if (craftItem.ResearchedByDefault)
                    res.Add(item as CraftingItem);
            }
            return res;
        }
    }
}
