using Multiplayer.Multiplay_Instances;
using UnityEngine;

public class InHandExplosive : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerExplosiveThrow.singleton.SetCurrentId(GetComponent<MultiplayInstanceId>().Id);
        CharacterUIHandler.singleton.ActivateThrowButton(true);
    }

    private void OnDisable()
    {
        PlayerExplosiveThrow.singleton.SetCurrentId(-1);
        CharacterUIHandler.singleton.ActivateThrowButton(false);
    }
}