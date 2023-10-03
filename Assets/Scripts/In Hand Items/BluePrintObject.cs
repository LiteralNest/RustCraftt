using UnityEngine;

public class BluePrintObject : MonoBehaviour
{
    private void OnEnable()
        => GlobalEventsContainer.BluePrintActiveSelfSet?.Invoke(true);

    private void OnDisable()
        => GlobalEventsContainer.BluePrintActiveSelfSet?.Invoke(false);
}