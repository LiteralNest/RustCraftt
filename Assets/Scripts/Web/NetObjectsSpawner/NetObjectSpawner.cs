using Unity.Netcode;
using UnityEngine;

namespace Web.NetObjectsSpawner
{
    public class NetObjectSpawner : MonoBehaviour
    {
        [SerializeField] private string _prefabPath = "NetObject/";
        
        public void GeneratePref(Transform parent)
        {
            var position = transform.position;
            Debug.Log("Generating " + _prefabPath + " at " + position);
            ClearChildren();
            var prefab = Resources.Load<GameObject>(_prefabPath);
            var instance = Instantiate(prefab, position, Quaternion.identity);
            var networkObject = instance.GetComponent<NetworkObject>();
            networkObject.Spawn();
            networkObject.TrySetParent(parent);
        }
        
        private void ClearChildren()
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
        }
    }
}