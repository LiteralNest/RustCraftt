using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer.Multiplay_Instances
{
    public class MultiplayObjectsPool : MonoBehaviour
    {
        public static MultiplayObjectsPool singleton { get; private set; }
        
        private void Awake()
            => singleton = this;
        
        [SerializeField] private List<MultiplayInstanceId> _multiplayInstanceIds = new List<MultiplayInstanceId>();
     
        public MultiplayInstanceId GetMultiplayInstanceIdById(int id)
            => _multiplayInstanceIds.Find(x => x.Id == id);
        
    }
}
