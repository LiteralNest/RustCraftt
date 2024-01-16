using Multiplayer;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class PlayerCorpesHanler : NetworkBehaviour
    {
        [SerializeField] private NetworkObject _corpesPref;
        [SerializeField] private PlayerCorpes _corpesObject;
        [SerializeField] private Transform _corpesPlace;

        public void GenerateBackPack(CustomSendingInventoryData data, bool wasDisconnected, int ownerId)
        {
            var backPack = Instantiate(_corpesPref.gameObject, _corpesPlace.position, _corpesPlace.rotation);
            backPack.GetComponent<NetworkObject>().Spawn();
            var script = backPack.GetComponent<BackPack>();
            script.AssignCells(data);
            script.PlayerCorpDisplay.Init();
            script.SetWasDisconnectedAndOwnerId(wasDisconnected, ownerId);
        }
    }
}