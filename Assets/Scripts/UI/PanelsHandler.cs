using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PanelsHandler : MonoBehaviour
    {
        public void TurnOffPanel(GameObject panel)
            => panel.SetActive(false);

        public void TurnOnPanel(GameObject panel)
            => panel.SetActive(true);
        
        public void DeactivateButton(Button button) => button.interactable = false;

        public void ActivateButton(Button button) => button.interactable = true;

        public void SetCanDragInventoryItems(bool canDrag)
            => GlobalValues.CanDragInventoryItems = canDrag;
        
        public void Quit()
            => Application.Quit();

        public void LoadLevel(int id)
            => SceneManager.LoadScene(id);

        public void LoadLevelAsync(int id)
            => LevelsLoader.singleton.LoadLevelAsync(id);
    }
}