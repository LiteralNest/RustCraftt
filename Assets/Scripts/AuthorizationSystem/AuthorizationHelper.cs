using System.Collections.Generic;
using System.Linq;
using Multiplayer.CustomData;
using Unity.Netcode;

namespace AuthorizationSystem
{
    public class AuthorizationHelper
    {
        public bool IsAuthorized(int value, NetworkVariable<AuthorizedUsersData> authorizedIds)
        {
            var list = authorizedIds.Value.AuthorizedIds.ToList();
            return list.Contains(value);
        }

        private int[] GetConvertedArray(List<int> list)
        {
            int[] array = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = list[i];
            return array;
        }
        

        public void Authorize(int id, NetworkVariable<AuthorizedUsersData> authorizedIds)
        {
            var cache = new NetworkVariable<AuthorizedUsersData>(authorizedIds.Value);
            var list = cache.Value.AuthorizedIds.ToList();
            list.Add(id);
            authorizedIds.Value = new AuthorizedUsersData(GetConvertedArray(list));
        }
    }
}