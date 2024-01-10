using System.Collections;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Web.NetObjectsSpawner
{
    public class NetObjectSpawnersContainer : NetworkBehaviour
    {
        [SerializeField] private float _delayBetweenSpawning = 0.01f;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsServer) return;
            StartCoroutine(LoadSpawnersRoutine());
        }

        private IEnumerator LoadSpawnersRoutine()
        {
            var spawners = FindObjectsOfType<NetObjectSpawner>().ToList();
            foreach (var spawner in spawners)
            {
                spawner.GeneratePref();
                yield return new WaitForSeconds(_delayBetweenSpawning);
            }
        }
    }
}