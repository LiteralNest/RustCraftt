using UnityEngine;

namespace Cloud.DataBaseSystem.UserData
{
   [System.Serializable]
   public class UserData
   {
      [SerializeField] private int id;
      [SerializeField] private string name;
      [SerializeField] private int _goldValue;

      public int Id => id;
      public string Name => name;
      public int GoldValue => _goldValue;

      public UserData(int id, string name, int goldValue)
      {
         this.id = id;
         this.name = name;
         _goldValue = goldValue;
      }
      
      
   }
}