using UnityEngine;

namespace Items_System.Ore_Type
{
    public class CharacterOre : ResourceOre
    {
        [SerializeField] private GameObject _activatingObject;
    
        protected override void DestroyObject()
        {
            TurnRenderers(false);
            _activatingObject.SetActive(true);
        }
    }
}
