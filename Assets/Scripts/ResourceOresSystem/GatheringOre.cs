using Unity.Netcode;
using UnityEngine;

namespace ResourceOresSystem
{
    [RequireComponent(typeof(BoxCollider))]
    public class GatheringOre : Ore
    {
        [SerializeField] private bool _shouldDelete;
    
        private void Start()
        {
            gameObject.tag = "Gathering";
        }

        public void Gather()
        {
            if(Recovering) return;
            AddResourcesToInventory();
            MinusHpServerRpc();
            if (_shouldDelete)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}