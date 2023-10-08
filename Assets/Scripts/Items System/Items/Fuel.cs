using UnityEngine;
[CreateAssetMenu(menuName = "Item/Fuel")]
public class Fuel : Resource
{
    [field: SerializeField] public float BurningTime { get; private set; }
}
