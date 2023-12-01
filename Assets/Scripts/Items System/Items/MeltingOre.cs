using Items_System.Items.Abstract;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Melting Ore")]
    public class MeltingOre : Item
    {
        [Header("Melting Ore")]
        public float MeltingTime;
        public MeltedOre Result;
    }
}
