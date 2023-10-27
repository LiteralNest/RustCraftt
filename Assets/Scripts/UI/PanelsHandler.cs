using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelsHandler : MonoBehaviour
{
    public void TurnOffPanel(GameObject panel)
        => panel.SetActive(false);

    public void TurnOnPanel(GameObject panel)
        => panel.SetActive(true);

    public void SetCanDragInventoryItems(bool canDrag)
        => GlobalValues.CanDragInventoryItems = canDrag;
        
    public void Quit()
        => Application.Quit();

    public void LoadLevel(int id)
        => SceneManager.LoadScene(id);
}