using UnityEngine;
using UnityEngine.Serialization;

namespace Cloud.DataBaseSystem.UserData
{
   [System.Serializable]
   public class UserData
   {
      [SerializeField] private int id;
      [SerializeField] private string name;

      public int Id => id;
      public string Name => name;

      public UserData(int id, string name)
      {
         this.id = id;
         this.name = name;
      }
   }
}