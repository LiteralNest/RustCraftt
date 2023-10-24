public class InventorySlotsContainer : SlotsContainer
{
    public static InventorySlotsContainer singleton { get; private set; }

    private void Awake()
        => singleton = this;
}