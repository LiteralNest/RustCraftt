using System.Collections.Generic;
using AI.Animals;
using UnityEngine;

namespace AI
{
   [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
   public class AIPerception: MonoBehaviour
   {
      [SerializeField] private AnimalController _animalController;
      
      private List<GameObject> _targets = new List<GameObject>();

      private void Awake()
      {
         GetComponent<BoxCollider>().isTrigger = true;
         GetComponent<Rigidbody>().useGravity = false;
      }

      public List<T> GetObjects<T>() where T : class
      {
         var res = new List<T>();
         foreach (var target in _targets)
         {
            var component = target.GetComponent<T>();
            if (component == null) continue;
            res.Add(component);
         }
         return res;
      }

      private void OnTriggerEnter(Collider other)
      {
         if(_targets.Contains(other.gameObject)) return;
         _targets.Add(other.gameObject);
         _animalController.RefreshList();
      }

      private void OnTriggerExit(Collider other)
      {
         if(!_targets.Contains(other.gameObject)) return;
         _targets.Remove(other.gameObject);
         _animalController.RefreshList();
      }
   }
}
