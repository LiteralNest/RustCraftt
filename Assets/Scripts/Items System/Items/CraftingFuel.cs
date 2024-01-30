using Items_System.Items.Abstract;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/CraftingFuel")]
    public class CraftingFuel : CraftingItem
    {
        [field: SerializeField] public float BurningTime { get; private set; }
    }
}