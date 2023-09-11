public class InHandObtainingObject : InHandObject
{
    private void OnEnable()
    {
        GlobalEventsContainer.GatherButtonActivated?.Invoke(true);
    }
    
    private void OnDisable()
    {
        GlobalEventsContainer.GatherButtonActivated?.Invoke(false);
    }
}
