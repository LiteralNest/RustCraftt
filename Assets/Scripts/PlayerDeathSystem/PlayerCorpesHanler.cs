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

        public void ResetCorpesPos(int corpesId)
        {
            _corpesObject.transform.SetParent(null);
            _corpesObject.Id = corpesId;
        }

        public void MoveCorpes(CustomSendingInventoryData data, int corpesId)
        {
            var backPack = Instantiate(_corpesPref.gameObject, _corpesPlace.position, _corpesPlace.rotation);
            backPack.GetComponent<NetworkObject>().Spawn();
            backPack.GetComponent<BackPack>().AssignCells(data);
            backPack.GetComponent<BackPack>().AssignCorpServerRpc(corpesId);
        }
    }
}