using UnityEngine;

public class PlayerExplosiveThrow : MonoBehaviour
{
    [SerializeField] private GameObject _explosivePrefab;
    [SerializeField] private GameObject _landminePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _throwForce = 10f;

    private void SpawnAndThrowExplosive(GameObject obj)
    {
        var explosive = Instantiate(obj, _spawnPoint.position, Quaternion.identity);
        var rb = explosive.GetComponent<Rigidbody>();

        if (rb != null) rb.AddForce(transform.forward * _throwForce, ForceMode.Impulse);
    }

    // private void OnGUI()
    // {
    //     if (GUI.Button(new Rect(Screen.width - 500, Screen.height / 2 - 25, 200, 100), "Explosive"))
    //         SpawnAndThrowExplosive(_explosivePrefab);
    //     
    //     if (GUI.Button(new Rect(Screen.width - 500, Screen.height / 2 + 100, 200, 100), "Mine"))
    //         SpawnAndThrowExplosive(_landminePrefab);
    // }
}