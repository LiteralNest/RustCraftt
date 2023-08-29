using TMPro;
using UnityEngine;

public class ObjectsRayCaster : MonoBehaviour
{
    [field: SerializeField] public ResourceOre TargetResourceOre { get; private set; }
    [field: SerializeField] public GatheringOre TargetGathering { get; private set; }
    [field: SerializeField] public LootBox TargetBox { get; private set; }

    [Header("UI")] [SerializeField] private TMP_Text _lootText;
    
    [Header("Distances")]
    [SerializeField] private float _maxOreHitDistance = 3f;
    [SerializeField] private float _maxGatheringDistance = 5f;
    [SerializeField] private float _maxLootBoxDistance = 5f;

    private void FixedUpdate()
    {
        TryRaycastTargets();
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
    

    private void SetLootText(string text)
    {
        if (!_lootText.gameObject.activeSelf)
            _lootText.gameObject.SetActive(true);
        _lootText.text = text;
    }
    
    private void TryRaycastTargets()
    {
        _lootText.gameObject.SetActive(false);
        TargetBox = null;
        TargetGathering = null;
        TargetResourceOre = null;

        if (TryRaycast("Gathering", _maxGatheringDistance, out GatheringOre item))
        {
            TargetGathering = item;
            SetLootText("Gather");
            return;
        }

        if (TryRaycast("LootBox", _maxLootBoxDistance, out LootBox lootBox))
        {
            TargetBox = lootBox;
            SetLootText("Open");
            return;
        }
        
        if (TryRaycast("Ore", _maxOreHitDistance, out ResourceOre ore))
        {
            TargetResourceOre = ore;
            SetLootText("Obtain");
            return;
        }
    }
}
