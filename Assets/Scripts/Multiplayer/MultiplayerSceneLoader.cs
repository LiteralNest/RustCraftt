using UnityEngine;
using UnityEngine.SceneManagement;

namespace Multiplayer
{
    public class MultiplayerSceneLoader : MonoBehaviour
    {
#if UNITY_SERVER
    private void Start()
        => SceneManager.LoadScene(1);
#endif
    }
}