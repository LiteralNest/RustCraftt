using System.Collections;
using AI.Animals;
using AI.Animals.AnimalsSpawner;
using UnityEngine;

namespace ResourceOresSystem
{
    public class AnimalsCorpOre : CorpOre
    {
        [Header("AnimalsCorpOre")]
        [SerializeField] private AnimalID _targetAnimal;

        protected override IEnumerator DestroyRoutine()
        {
            AnimalsSpawner.Singleton.RespawnAnimal(_targetAnimal);
            yield return base.DestroyRoutine();
        }
    }
}