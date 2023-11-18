using System.Threading.Tasks;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class UserCreator : MonoBehaviour
{
   [Header("Attached Scripts")]
   [SerializeField] private UserJsonDataHandler _userJsonDataHandler;
   [SerializeField] private UserDataHandler _userDataHandler;
   
   [Header("UI")] 
   [SerializeField] private GameObject _creatingUserPanel;
   [SerializeField] private TMP_InputField _userInputField;
   [SerializeField] private TMP_Text _userExistsText;
   private DatabaseReference _databaseReference;
   [SerializeField] private GameObject _createUserButoon;
   private void Start()
      => _databaseReference = FirebaseSetup.singleton.DatabaseReference;

   public void Init(bool exists, UserData data)
   {
      if (!exists)
      {
         _creatingUserPanel.SetActive(true);
         return;
      }
      _userDataHandler.UserData = data;
   }
   
   public async void TryCreateUser()
   {
      int id = await GetLastUserId();
      _userExistsText.gameObject.SetActive(false);
      _createUserButoon.SetActive(false);
      string name = _userInputField.text;
      var userExists = await UserExists(name);
      if (userExists)
      {
         _createUserButoon.SetActive(true);
         _userExistsText.gameObject.SetActive(true);
         return;
      }
      
      UserData data = new UserData();
      data.Name = name;
      data.Id = ++id;
      string json = JsonUtility.ToJson(data);
      _userJsonDataHandler.SaveUserData(json);
      await _databaseReference.Child("Users").Child(data.Id.ToString()).SetRawJsonValueAsync(json);
      _userDataHandler.UserData = data;
      _creatingUserPanel.SetActive(false);
   }
   
   public async Task<bool> UserExists(string name)
   { 
      var task = await _databaseReference.Child("Users").OrderByChild("Name").EqualTo(name).GetValueAsync();
      if(task.Exists) return true;
      return false;
   }
   
   public async Task<int> GetLastUserId()
   {
      var task = await _databaseReference.Child("Users").OrderByKey().LimitToLast(1).GetValueAsync();
      if (task.ChildrenCount == 0) return 0;
      foreach (var child in task.Children)
      {
         UserData data = JsonUtility.FromJson<UserData>(child.GetRawJsonValue());
         return data.Id;
      }
      
      return 0;
   }
}
