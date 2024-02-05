using System.Collections.Generic;
using UnityEngine;

namespace Building_System.Blocks
{
    public class Block : MonoBehaviour
    {
        [field: SerializeField] public int Hp { get; protected set; }
        [field: SerializeField] public List<InventoryCell> CellForPlace { get; private set; }
        [field: SerializeField] public List<InventoryCell> CellsForRemovingPerTime { get; private set; }

        [Header("Sounds")] 
        [SerializeField] private AudioClip _damageSound;
        public AudioClip DamageSound => _damageSound;
        [SerializeField] private AudioClip _destroyingSound;
        public AudioClip DestroyingSound => _destroyingSound;
        [field:SerializeField] public AudioClip UpgradingSound { get; private set; }
    }
}
