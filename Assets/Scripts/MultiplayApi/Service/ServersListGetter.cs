using MultiplayApi.Service;
using Server;
using UnityEngine;

public class ServersListGetter : MonoBehaviour
{
   [SerializeField] private ConnectionManager _connectionManager;

   private async void GetServersList()
   {
      ServerData[] serverData = new ServerData[10];
      Debug.Log("ServersGetted");
   }
}
