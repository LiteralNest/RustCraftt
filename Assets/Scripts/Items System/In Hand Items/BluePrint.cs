using UnityEngine;

public class BluePrint : MonoBehaviour
{
    private void OnEnable()
        => GlobalEventsContainer.BluePrintActiveSelfSet?.Invoke(true);

    private void OnDisable()
        => GlobalEventsContainer.BluePrintActiveSelfSet?.Invoke(false);
}