using System.Collections.Generic;
using AI.Animals;
using Unity.Netcode;
using UnityEngine;

namespace Multiplayer
{
    public class AnimalObjectInstantiator : NetworkBehaviour
    {
        public static AnimalObjectInstantiator singleton { get; private set; }
    
        [SerializeField] private List<AnimalID> _animalIds = new List<AnimalID>();

        private void Awake()
            => singleton = this;
        
        private void Spawn(AnimalID targetPref, Vector3 position, Vector3 rotation)
        {
            var animal = Instantiate(targetPref, position, Quaternion.Euler(rotation));
            NetworkObject networkObject = animal.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }

        public void SpawnAnimalCorpById(int id, Vector3 position, Vector3 rotation)
        {
            foreach (var animal in _animalIds)
            {
                if (animal.Id == id)
                {
                    Spawn(animal, position, rotation);
                    return;
                }
            }
            Debug.LogError("Can't find object with id: " + id);
        }
    }
}