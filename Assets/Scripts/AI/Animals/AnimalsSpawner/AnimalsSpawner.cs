using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace AI.Animals.AnimalsSpawner
{
    public class AnimalsSpawner : MonoBehaviour
    {
        public static AnimalsSpawner Singleton { get; set; }

        [Header("Main Parameters")] [SerializeField]
        private List<AnimalSpawnerSlot> _animalSlots = new List<AnimalSpawnerSlot>();

        [SerializeField] private List<GameObject> _placeForAnimal = new List<GameObject>();

        private void Awake()
            => Singleton = this;

        public void RespawnAnimal(AnimalID animalId)
        {
            foreach (var animal in _animalSlots)
            {
                if (animalId.Id != animal.TargetAnimal.Id) continue;
                StartCoroutine(RespawnAnimalRoutine(animal));
            }
        }

        private IEnumerator RespawnAnimalRoutine(AnimalSpawnerSlot slot)
        {
            yield return new WaitForSeconds(slot.RespawnTime);
            var randPoint = _placeForAnimal[Random.Range(0, _placeForAnimal.Count)];
            var instance = Instantiate(_placeForAnimal[Random.Range(0, _placeForAnimal.Count)]);
            instance.transform.position = randPoint.transform.position;
            instance.GetComponent<NetworkObject>().Spawn();
        }
    }
}