using UnityEngine;

[System.Serializable]
public class BuildingRendererSlot
{
    public Renderer Renderer { get; private set; }
    public Material ReservedMaterial { get; private set; }

    public BuildingRendererSlot(Renderer renderer, Material reservedMaterial)
    {
        Renderer = renderer;
        ReservedMaterial = reservedMaterial;
    }
}
