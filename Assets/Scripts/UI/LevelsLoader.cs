using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsLoader : MonoBehaviour
{
   public static LevelsLoader singleton { get; private set; }
   
   [SerializeField] private GameObject _loadScreenPanel;
 
   private void Awake()
   {
      if(singleton != null && singleton != this)
         Destroy(this);
      singleton = this;
      DontDestroyOnLoad(this);
   }

   public async void LoadLevelAsync(int id)
   {
      _loadScreenPanel.SetActive(true);
      await SceneManager.LoadSceneAsync(id);
      _loadScreenPanel.SetActive(false);
   }

   public void CancelLoading()
   {
      SceneManager.LoadScene(0);
      _loadScreenPanel.SetActive(false);
   }
}
