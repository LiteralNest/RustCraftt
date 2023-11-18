using System.Threading.Tasks;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField] private float _destroyingTime = 1;

    private async void Start()
    {
        await Task.Delay((int)(_destroyingTime * 1000));
        Destroy(gameObject);
    }
}
