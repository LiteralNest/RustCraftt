using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField] private float _destroyingTime = 1;

    private void Start()
    {
        Destroy(gameObject, _destroyingTime);
    }
}