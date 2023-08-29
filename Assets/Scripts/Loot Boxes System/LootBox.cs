using UnityEngine;

public class LootBox : MonoBehaviour
{
    [SerializeField] private 
    [SerializeField] private LootBoxGeneratingSet _set;

    private void GenerateItems(Transform place)
    {
        
    }
    
    public void Open()
    {
        InventoryCellsDisplayer.singleton.OpenLootBoxPanel();
    }
}
