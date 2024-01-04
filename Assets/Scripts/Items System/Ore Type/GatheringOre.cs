using Unity.Netcode;
using UnityEngine;

namespace Items_System.Ore_Type
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
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