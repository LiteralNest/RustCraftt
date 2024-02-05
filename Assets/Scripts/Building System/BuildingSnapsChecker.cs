using System.Collections.Generic;
using FightSystem.Damage;
using Unity.Netcode;
using UnityEngine;

namespace Building_System
{
    public class BuildingSnapsChecker : MonoBehaviour
    {
        [SerializeField] private NetworkObject _destroyingObject;
        private List<BuildingSnapsChecker> _snapObjects = new List<BuildingSnapsChecker>();
        private GameObject _ground;

        private void FilterList()
        {
            for (int i = 0; i < _snapObjects.Count; i++)
            {
                if (_snapObjects[i] != null) return;
                _snapObjects.RemoveAt(i);
                i--;
            }
        }

        private bool ThereIsGround()
        {
            foreach(var snap in _snapObjects)
                if (snap._ground != null) return true;
            return false;
        }
        
        private void CheckSnaps()
        {
            if(_ground != null) return;
            foreach (var snap in _snapObjects)
                if (snap.ThereIsGround())
                    return;
            _destroyingObject.GetComponent<IDamagable>().Destroy();
        }

        private void OnTriggerEnter(Collider other)
        {
            FilterList();
            var otherGO = other.gameObject;
            if (otherGO.CompareTag("Ground"))
            {
                _ground = otherGO;
                return;
            }

            var otherSnap = otherGO.GetComponent<BuildingSnapsChecker>();
            if (!otherSnap || _snapObjects.Contains(otherSnap)) return;
            _snapObjects.Add(otherSnap);
        }

        private void OnTriggerExit(Collider other)
        {
            FilterList();
            var otherGO = other.gameObject;
            if (_ground == otherGO)
            {
                _ground = null;
                return;
            }

            var otherSnap = otherGO.GetComponent<BuildingSnapsChecker>();
            if (!otherSnap || !_snapObjects.Contains(otherSnap)) return;
            _snapObjects.Remove(otherSnap);
            CheckSnaps();
        }
    }
}