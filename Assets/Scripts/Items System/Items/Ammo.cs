using UnityEngine;

[CreateAssetMenu(menuName = "Item/Ammo")]
public class Ammo : CraftingItem
{
    public float MultiplyKoef => _multiplyKoef;
    [Range(1, 5)]
    [Header("Ammo")] [SerializeField] private float _multiplyKoef = 1;
}