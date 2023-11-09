using UnityEngine;

[CreateAssetMenu(menuName = "Item/Cooking Food")]
public class CookingFood : Food
{
    [Header("Cooking food")]
    [SerializeField] private float _cookingTime;
    public float CookingTime => _cookingTime;
    [field: SerializeField] public Food FoodAfterCooking { get; private set; }
}