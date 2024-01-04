using ArmorSystem.Backend;
using UnityEngine;

namespace Items_System.Items
{
    [CreateAssetMenu(menuName = "Item/Armor")]
    public class Armor : DamagableItem
    {
        [Header("Armor")] [SerializeField] private BodyPartType _bodyPartType;
        public BodyPartType BodyPartType => _bodyPartType;

        [Range(0, 100)] [SerializeField] private int _radiationResistValue;
        public int RadiationResistValue => _radiationResistValue;

        [Range(0, 100)] [SerializeField] private int _hitResistValue;
        public int HitResistValue => _hitResistValue;

        [Range(0, 100)] [SerializeField] private int _temperatureResistValue;
        public int TemperatureResistValue => _temperatureResistValue;

        [Range(0, 100)] [SerializeField] private int _explosionResistValue;
        public int ExplosionResistValue => _explosionResistValue;
    }
}