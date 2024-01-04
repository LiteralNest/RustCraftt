using Items_System.Items.Abstract;
using UnityEngine;

namespace Items_System.Items
{
    public class DamagableItem : CraftingItem
    {
      [field:SerializeField]  public int Hp { get; set; }
    }
}
