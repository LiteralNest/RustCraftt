using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.LoadingSceneUI
{
    public class LoadingSceneView : MonoBehaviour
    {
        [SerializeField] private Button _cancelButton;

        private void Start()
        {
            _cancelButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }
    }
}