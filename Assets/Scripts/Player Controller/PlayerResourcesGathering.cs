using System.Threading.Tasks;
using UnityEngine;

public class PlayerResourcesGathering : MonoBehaviour
{
    [SerializeField] private InventorySlotsContainer _inventory;
    [SerializeField] private float _maxHitDistance = 3f;
    [SerializeField] private float _recoveringSpeed = 0.3f;
    [SerializeField] private bool _canHit = true;
    private bool _gathering = false;

    private void Update()
    {
        if(_gathering)
            TryHit();
    }
    
    private async void Recover()
    {
        _canHit = false;
        await Task.Delay((int)(_recoveringSpeed * 1000));
        _canHit = true;
    }

    public void StartGathering()
    {
        _gathering = true;
    }
    
    public void StoppGathering()
    {
        _gathering = false;
    }

    private void TryHit()
    {
        if (!_canHit) return;
        Recover();
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, _maxHitDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            if (!hitObject.TryGetComponent(out ResourceOre ore)) return;
            ore.MinusHp(_inventory);
        }
    }
}