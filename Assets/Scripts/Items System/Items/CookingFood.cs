using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Cooking Food")]
    public class CookingFood : Food.Food
    {
        [Header("Cooking food")]
        [SerializeField] private float _cookingTime;
        public float CookingTime => _cookingTime;
        [field: SerializeField] public Food.Food FoodAfterCooking { get; private set; }
    }
}