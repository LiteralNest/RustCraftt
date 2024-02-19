using Sirenix.OdinInspector;
using UnityEngine;

namespace Cloud.DataBaseSystem.DataBaseServices.ServerData
{
    public class ServerDataBaseView : MonoBehaviour
    {
        [Header("Test")]
        [SerializeField] private string _ip;
        [SerializeField] private string _playersCount;

        [Button]
        private void Test()
        {
            ServerDataBaseHandler serverDataBaseHandler = new();
            serverDataBaseHandler.UpdateServerDataAsync(_ip, _playersCount);
        }
    }
}