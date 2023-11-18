using UnityEngine;

public class SpearFacade : MonoBehaviour
{
    private void OnEnable()
    {
        GlobalEventsContainer.ShouldHandleScopeSpear?.Invoke(true);
    }
    
    private void OnDisable()
    {
        GlobalEventsContainer.ShouldHandleScopeSpear?.Invoke(false);
    }
}