using System.Collections.Generic;
using UnityEngine;

namespace Building_System.Blocks
{
    public class Block : MonoBehaviour
    {
        [field: SerializeField] public int Hp { get; protected set; }
        [field: SerializeField] public List<InventoryCell> CellForPlace { get; private set; }
        [field: SerializeField] public List<InventoryCell> CellsForRemovingPerTime { get; private set; }
        [field:SerializeField] public AudioClip UpgradingSound { get; private set; }
    }
}
