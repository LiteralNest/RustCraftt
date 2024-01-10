using System.Collections;
using UnityEngine;


namespace Optimization
{
    public class AsyncSceneObjectsLoader : MonoBehaviour
    {
        [SerializeField] private string _objectsPath = "StartSceneObjects";
        [SerializeField] private Transform _parrent;
        [SerializeField] private float _waitingPerObjectTime;

        private void Start()
        =>StartCoroutine(LoadSceneObjectsAsync());

        private IEnumerator LoadSceneObjectsAsync()
        {
            var objects = Resources.LoadAll<GameObject>(_objectsPath);
            foreach (var prefab in objects)
            {
                yield return new WaitForSeconds(_waitingPerObjectTime);
                Instantiate(prefab, _parrent);
            }
        }
    }
}