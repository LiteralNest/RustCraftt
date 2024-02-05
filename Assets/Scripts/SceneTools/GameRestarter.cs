using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneTools
{
    public class GameRestarter : MonoBehaviour
    {
        private void OnApplicationPause(bool pauseStatus)
            => SceneManager.LoadScene(0);
    }
}