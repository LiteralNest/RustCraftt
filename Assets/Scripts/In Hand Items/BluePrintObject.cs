using UnityEngine;

public class BluePrintObject : MonoBehaviour
{
    private void OnEnable()
        => CharacterUIHandler.singleton.ActivateBuildingButton(true);

    private void OnDisable()
        => CharacterUIHandler.singleton.ActivateBuildingButton(false);
}