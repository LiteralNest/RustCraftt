using UnityEngine;

public class PlayerExplosiveThrow : MonoBehaviour
{
    [SerializeField] private GameObject _explosivePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _throwForce = 10f;

    private void SpawnAndThrowExplosive()
    {
        var explosive = Instantiate(_explosivePrefab, _spawnPoint.position, Quaternion.identity);
        var rb = explosive.GetComponent<Rigidbody>();

        if (rb != null) rb.AddForce(transform.forward * _throwForce, ForceMode.Impulse);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 100, Screen.height / 2 - 25, 200, 100), "Throw Explosive"))
            SpawnAndThrowExplosive();
    }
}