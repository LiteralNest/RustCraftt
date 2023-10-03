using UnityEngine;

[CreateAssetMenu(menuName = "Item/Cooking Food")]
public class CookingFood : Food
{
    [field: SerializeField] public Food FoodAfterCooking { get; private set; }
}