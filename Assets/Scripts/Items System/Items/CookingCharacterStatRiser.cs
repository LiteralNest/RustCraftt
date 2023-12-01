using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Cooking Food")]
    public class CookingCharacterStatRiser : CharacterStatRiser
    {
        [Header("Cooking food")]
        [SerializeField] private float _cookingTime;
        public float CookingTime => _cookingTime;
        [field: SerializeField] public CharacterStatRiser CharacterStatRiserAfterCooking { get; private set; }
    }
}