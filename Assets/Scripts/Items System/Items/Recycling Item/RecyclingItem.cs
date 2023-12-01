using System.Collections.Generic;
using Items_System.Items.Abstract;
using UnityEngine;

namespace Items_System.Items.Recycling_Item
{
    [System.Serializable]
    public struct RecyclingItemCell
    {
        [field: SerializeField] public Item ResultItem { get; set; }
        [field: SerializeField] public Vector2Int ItemsRange { get; set; }
    }

    [CreateAssetMenu(menuName = "Item/Recycling Item")]
    public class RecyclingItem : Item
    {
        [field:SerializeField] public List<RecyclingItemCell> Cells { get; private set; }
    }
}