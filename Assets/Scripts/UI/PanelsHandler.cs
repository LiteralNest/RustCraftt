using UnityEngine;

public class PanelsHandler : MonoBehaviour
{
    public void TurnOffPanel(GameObject panel)
        => panel.SetActive(false);

    public void TurnOnPanel(GameObject panel)
        => panel.SetActive(true);

    public void SetCanDragInventoryItems(bool canDrag)
        => GlobalValues.CanDragInventoryItems = canDrag;
}