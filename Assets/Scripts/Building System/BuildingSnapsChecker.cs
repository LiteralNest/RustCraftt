using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Building_System
{
    [RequireComponent(typeof(BoxCollider))]
    public class BuildingSnapsChecker : MonoBehaviour
    {
        [SerializeField] private NetworkObject _destroyingObject;
        private List<GameObject> _snapObjects = new List<GameObject>();

        private void FilterList()
        {
            for (int i = 0; i < _snapObjects.Count; i++)
            {
                if(_snapObjects[i] != null) return;
                _snapObjects.RemoveAt(i);
                i--;
            }
        }
        
        private void CheckSnaps()
        {
            if(_snapObjects.Count > 0) return;
            _destroyingObject.GetComponent<IDamagable>().Destroy();
        }

        private void OnTriggerEnter(Collider other)
        {
            FilterList();
            var otherGO = other.gameObject;
            if (otherGO.CompareTag("ConnectingPoint") || otherGO == _destroyingObject.gameObject || _snapObjects.Contains(otherGO)) return;
            _snapObjects.Add(otherGO);
        }

        private void OnTriggerExit(Collider other)
        {
            FilterList();
            var otherGO = other.gameObject;
            if (!_snapObjects.Contains(otherGO)) return;
            _snapObjects.Remove(otherGO);
            CheckSnaps();
        }
    }
}