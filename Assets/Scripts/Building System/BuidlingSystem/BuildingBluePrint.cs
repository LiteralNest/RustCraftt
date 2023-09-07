using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class BuildingBluePrint : MonoBehaviour
{
    [Header("Target")] 
    [SerializeField] protected GameObject _pref;

    [Header("Colors")] 
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _badColor;
    
    [Header("Renderers")] 
    [SerializeField] private List<Renderer> _renderers = new List<Renderer>();

    [Header("Inventory Materials")] 
    [SerializeField] protected List<InventoryCell> _neededCellsForPlace = new List<InventoryCell>();

    public bool CanBePlaced { get; protected set; }
    
    private void OnEnable()
        => GlobalEventsContainer.InventoryDataChanged += CheckForAvailable;
    
    private void OnDisable()
        => GlobalEventsContainer.InventoryDataChanged -= CheckForAvailable;

    protected void Init()
        => CheckForAvailable();

    #region Abstract
    public abstract void CheckForAvailable();
    public abstract bool TryGetObjectCoords(Camera targetCamera, out Vector3 coords);
    
    #endregion

    #region Virtual
    public virtual void TriggerEntered(Collider other){}
    public virtual void TriggerExit(Collider other){}
    public void Place()
    {
        Instantiate(_pref, transform.position, transform.rotation);
    }
    #endregion

    private void SetColorToRenderers(Color color)
    {
        foreach (var renderer in _renderers)
        {
            Material mat = new Material(renderer.sharedMaterial);
            renderer.material = mat;
            renderer.material.color = color;
        }
    }

    protected void DisplayRenderers()
    {
        if (CanBePlaced)
        {
            SetColorToRenderers(_normalColor);
            return;
        }
        SetColorToRenderers(_badColor);
    }

    public bool TryPlace()
    {
        if (!CanBePlaced)
        {
            // Destroy(gameObject);
            return false;
        }
        Place();
        return true;
    }
    
    private void OnTriggerEnter(Collider other)
        => TriggerEntered(other);

    private void OnTriggerExit(Collider other)
        => TriggerExit(other);
}