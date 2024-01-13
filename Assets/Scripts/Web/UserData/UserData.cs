using UnityEngine;

namespace Web.UserData
{
   [System.Serializable]
   public class UserData
   {
      [SerializeField] private int _id;
      [SerializeField] private string _name;

      public int Id => _id;
      public string Name => _name;

      public UserData(int id, string name)
      {
         _id = id;
         _name = name;
      }
   }
}