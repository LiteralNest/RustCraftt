using Events;
using UI;
using UnityEngine;

namespace In_Hand_Items
{
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
}