using UnityEngine;

namespace AI.Animals.AnimalsSpawner
{
    [System.Serializable]
    public struct AnimalSpawnerSlot
    {
        [SerializeField] private AnimalID _targetAnimal;
        [SerializeField] private float _respawnTime;
        
        public AnimalID TargetAnimal => _targetAnimal;
        public float RespawnTime => _respawnTime;
    }
}