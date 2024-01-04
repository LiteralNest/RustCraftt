using Storage_System;
using UnityEngine;

namespace Items_System.Ore_Type
{
    public class CharacterOre : ResourceOre
    {
        [SerializeField] private BackPack _backPack;
        [SerializeField] private GameObject _activatingObject;
    
        private void Start(){
        
        }
        
        protected override void DestroyObject()
        {
            TurnRenderers(false);
            _activatingObject.SetActive(true);
            _backPack.SetWasDisconnectedAndOwnerIdServerRpc(false, -1);
        }
    }
}
