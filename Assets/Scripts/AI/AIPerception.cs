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

      public List<Transform> GetObjects<T>() where T : class
      {
         var res = new List<Transform>();
         foreach (var target in _targets)
         {
            if(target == null) continue;
            var component = target.GetComponent<T>();
            if (component == null) continue;
            res.Add(target.transform);
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
