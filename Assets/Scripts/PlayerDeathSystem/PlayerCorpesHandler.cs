using Cloud.CloudStorageSystem;
using Storage_System;
using Unity.Netcode;
using UnityEngine;

namespace PlayerDeathSystem
{
    public class PlayerCorpesHandler : NetworkBehaviour
    {
        [SerializeField] private Transform _corpesPlace;

        public void GenerateBackPack(CustomSendingInventoryData data, bool wasDisconnected, int ownerId,
            string nickName)
        {
            BackPackGenerator.Singleton.GenerateBackPack(wasDisconnected, ownerId, nickName, _corpesPlace.position, _corpesPlace.eulerAngles, data);
        }
    }
}