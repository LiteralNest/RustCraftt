using UnityEngine;

[CreateAssetMenu(menuName = "Item/Food")]
public class Food : Resource
{
    [SerializeField] private int _addingFood;
    
    public override void Click(InventoryHandler handler)
    {
        handler.Stats.PlusStat(CharacterStatType.Food, _addingFood);
    }
}