public class ToolClipboard : Storage
{
    public override void Open(InventoryHandler handler)
    {
        handler.ToolClipboardSlotsContainer.InitCells(Cells, this);
        handler.OpenClipBoardPanel();
    }
}