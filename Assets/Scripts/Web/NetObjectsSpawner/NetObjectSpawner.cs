using Unity.Netcode;
using UnityEngine;

namespace Web.NetObjectsSpawner
{
    public class NetObjectSpawner : MonoBehaviour
    {
        [SerializeField] private string _prefabPath = "NetObject/";
        
        public void GeneratePref()
        {
            var position = transform.position;
            Debug.Log("Generating " + _prefabPath + " at " + position);
            ClearChildren();
            var prefab = Resources.Load<GameObject>(_prefabPath);
            var instance = Instantiate(prefab, position, Quaternion.identity);
            instance.GetComponent<NetworkObject>().Spawn();
        }
        
        private void ClearChildren()
        {
            foreach(Transform child in transform)
                Destroy(child.gameObject);
        }
    }
}