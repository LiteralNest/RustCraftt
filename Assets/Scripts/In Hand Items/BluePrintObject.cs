using UI;
using UnityEngine;

public class BluePrintObject : MonoBehaviour
{
    private void OnEnable()
        => CharacterUIHandler.singleton.ActivateBuildingButton(true);

    private void OnDisable()
    {
        CharacterUIHandler.singleton.ActivateBuildingButton(false);
        CharacterUIHandler.singleton.ActivateBuildingChoosingPanel(false);
        GlobalEventsContainer.BluePrintDeactivated?.Invoke();
    }
}