using Items_System.Items.Abstract;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Ammo")]
    public class Ammo : CraftingItem
    {
        public float MultiplyKoef => _multiplyKoef;
        [Range(1, 5)]
        [Header("Ammo")] [SerializeField] private float _multiplyKoef = 1;
    }
}