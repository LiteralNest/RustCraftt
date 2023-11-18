using UnityEngine;

public class BluePrintObject : MonoBehaviour
{
    private void OnEnable()
        => MainUiHandler.singleton.ActivateBuildingButton(true);

    private void OnDisable()
        => MainUiHandler.singleton.ActivateBuildingButton(false);
}