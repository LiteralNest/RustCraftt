using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class BluePrint : MonoBehaviour
{
    [Header("Target")] 
    [SerializeField] protected int _poolPrefId;
    
    [Header("Colors")] 
    [SerializeField] protected Material _normalMaterial;
    [SerializeField] protected Material _negativeMaterial;
    
    [Header("Renderers")] 
    [SerializeField] protected List<Renderer> _renderers = new List<Renderer>();

    public bool CanBePlaced { get; protected set; }
    
    #region Abstract
    public abstract void CheckForAvailable();
    public abstract bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords);
    #endregion
    
    
    #region Virtual
    public virtual void TriggerEntered(Collider other){}
    public virtual void TriggerExit(Collider other){}
    #endregion

    private void SetMaterialToRenderers(Material material)
    {
        foreach (var renderer in _renderers)
            renderer.material = material;
    }
    
    protected void DisplayRenderers()
    {
        if (CanBePlaced)
        {
            SetMaterialToRenderers(_normalMaterial);
            return;
        }
        SetMaterialToRenderers(_negativeMaterial);
    }
    
    private void Place()
    {
        BuildingsNetworkingSpawner.singleton.SpawnPrefServerRpc(_poolPrefId, transform.position, transform.rotation);
    }
    
    public bool TryPlace()
    {
        if (!CanBePlaced)
            return false;
        Place();
        return true;
    }
    
    private void OnTriggerEnter(Collider other)
        => TriggerEntered(other);

    private void OnTriggerExit(Collider other)
        => TriggerExit(other);
}
