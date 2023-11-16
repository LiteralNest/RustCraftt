using Multiplayer.Multiplay_Instances;
using UnityEngine;

public class InHandExplosive : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerExplosiveThrow.singleton.SetCurrentId(GetComponent<MultiplayInstanceId>().Id);
        GlobalEventsContainer.ShouldDisplayThrowButton?.Invoke(true);
    }

    private void OnDisable()
    {
        PlayerExplosiveThrow.singleton.SetCurrentId(-1);
        GlobalEventsContainer.ShouldDisplayThrowButton?.Invoke(false);
    }
}