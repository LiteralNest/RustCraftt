using System.Collections.Generic;
using UnityEngine;

public class PlacingObjectBluePrint : MonoBehaviour
{
    [Header("Materials")] 
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _badMaterial;

    [Header("Start Init")] [SerializeField]
    private List<Renderer> _targetRenderers = new List<Renderer>();
    [field:SerializeField] public PlacingObject TargetPlacingObject { get; private set; }

    [Header("Layers")] [SerializeField] protected LayerMask _targetMask;
    
    private List<GameObject> _triggeredObjects = new List<GameObject>();

    private bool _rotatedSide;
    private bool _canBePlaced;
    private bool _canBePlacedFromPlayer;

    private void Start()
    {
        _canBePlaced = true;
        _canBePlacedFromPlayer = true;
    }
    
    private void SetMaterial(Material material)
    {
        foreach (var renderer in _targetRenderers)
        {
            renderer.material = new Material(material);
        }
    }

    private void DisplayRenderers()
    {
        if (!_canBePlaced || !_canBePlacedFromPlayer)
        {
            SetMaterial(_badMaterial);
            return;
        }
        SetMaterial(_normalMaterial);
    }

    public void TryPlace()
    {
        if (!_canBePlacedFromPlayer || !_canBePlaced) return;
        InventorySlotsContainer.singleton.DeleteSlot(TargetPlacingObject.TargetItem, 1);
        PlacingObjectsPool.singleton.InstantiateObjectServerRpc(TargetPlacingObject.TargetItem.Id, transform.position,
            transform.rotation);
        
    }

    public void Rotate()
    {
        transform.eulerAngles += new Vector3(0, 90, 0);
        _rotatedSide = !_rotatedSide;
    }

    public bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords)
    {
        Vector3 rayOrigin = targetCamera.transform.position;
        Vector3 rayDirection = targetCamera.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, _targetMask))
        {
            if (hit.transform.CompareTag("Block"))
            {
                int x, y, z;
                var structureSize = TargetPlacingObject.ObjectSize;

                // y = Mathf.RoundToInt(hit.point.y + hit.normal.y);
                var y1 = hit.point.y;
                y = Mathf.RoundToInt(y1);

                if (_rotatedSide)
                {
                    x = Mathf.RoundToInt(hit.point.x + hit.normal.x);
                    z = Mathf.RoundToInt(hit.point.z + hit.normal.z);
                }
                else
                {
                    x = Mathf.RoundToInt(hit.point.x + hit.normal.x);
                    z = Mathf.RoundToInt(hit.point.z + hit.normal.z);
                }

                coords = new Vector3(x, y, z);
                return true;
            }
        }

        coords = default;
        return false;
    }

    public void SetCanBePlaced(bool value)
    {
        _canBePlacedFromPlayer = value;
        DisplayRenderers();
    }

    private void OnTriggerEnter(Collider other)
    {
        _triggeredObjects.Add(other.gameObject);
        _canBePlaced = false;
        DisplayRenderers();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_triggeredObjects.Contains(other.gameObject))
            _triggeredObjects.Remove(other.gameObject);
        if (_triggeredObjects.Count != 0) return;
        _canBePlaced = true;
        DisplayRenderers();
    }
}