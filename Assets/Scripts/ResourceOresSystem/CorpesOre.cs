using System.Collections.Generic;
using Items_System.Ore_Type;
using UnityEngine;

namespace ResourceOresSystem
{
    public class CorpesOre : ResourceOre
    {
        [SerializeField] private List<GameObject> _displayingObjects = new();
        [SerializeField] private List<GameObject> _activatingObjects = new();

        protected override void DoAfterDestroying()
        {
            foreach(var obj in _displayingObjects)
                obj.SetActive(false);
            foreach(var obj in _activatingObjects)
                obj.SetActive(true);
        }
    }
}