using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerResourcesGatherer : MonoBehaviour
{
    [Header("Ore")]
    [SerializeField] private InventorySlotsContainer _inventory;
    [SerializeField] private float _maxOreHitDistance = 3f;
    [SerializeField] private float _recoveringSpeed = 0.3f;
    [SerializeField] private bool _canHit = true;
    
    [Header("Gathering")]
    [SerializeField] private float _maxGatheringDistance = 5f;
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
    
    public void StopGathering()
    {
        _gathering = false;
    }
    
    private bool TryRaycast<T>(string tag, float hitDistance, out T target)
    {
        target = default;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, hitDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if(!hitObject.CompareTag(tag)) return false;
            if (!hitObject.TryGetComponent(out T ore)) return false;
            target = ore;
            return true;
        }

        return false;
    }
    
    private void TryHit()
    {
        if (!_canHit) return;
        Recover();
        if(!TryRaycast("Ore", _maxOreHitDistance, out ResourceOre ore)) return;
        ore.MinusHp(_inventory);
    }

    public void TryGather()
    {
        if(!TryRaycast("Gathering", _maxOreHitDistance, out GatheringOre ore)) return;
        ore.Gather(_inventory);
    }
}