using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animation_System;
using CharacterStatsSystem;
using Multiplayer.PlayerSpawning;
using Player_Controller;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Multiplayer
{
    public class PlayerNetworkController : MonoBehaviour
    {
        [Header("Attached Components")]
        [SerializeField] private CharacterStats _characterStats; 
        [SerializeField] private PlayerEffectsHandler _playerEffectsHandler;
        [Header("NetCode")]
        [SerializeField] private PlayerNetCode _playerNetCode;
        [SerializeField] private CharacterAnimationsHandler _characterAnimationsHandler;
        [SerializeField] private CharacterAnimationsHandler _inventoryCharacterAnimationsHandler;
        [SerializeField] private List<Behaviour> _monos = new List<Behaviour>();
        [SerializeField] private List<GameObject> _disablingObjects = new List<GameObject>();

        [FormerlySerializedAs("_mainUiHandler")] [SerializeField]
        private CharacterUIHandler characterUIHandler;

        [Header("Children")] [SerializeField] private List<Renderer> _body = new List<Renderer>();
        [SerializeField] private GameObject _characterStaff;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private GameObject _playerNickName;

        private void Awake()
        {
            foreach (var mono in _monos)
                mono.enabled = false;
            characterUIHandler.enabled = false;
            _canvas.SetActive(false);
        }

        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(1);
            var players = FindObjectsOfType<PlayerStartSpawner>().ToList();
            foreach (var player in players)
            {
                if (GetComponent<NetworkObject>().OwnerClientId == player.GetComponent<NetworkObject>().OwnerClientId)
                {
                    player.AnimationsManager.CharacterAnimationsHandler = _characterAnimationsHandler;
                    player.AnimationsManager.InventoryAnimationsHandler = _inventoryCharacterAnimationsHandler;
                }
            }

            Destroy(this);
        }

        public void Start()
        {
            _canvas.SetActive(true);
            if (!_playerNetCode.IsOwner)
            {
                EnableMonos(false);
                ClearObjects();
                _playerEffectsHandler.CanInteract = false;
            }
            else
            {
                _playerNickName.SetActive(false);
                SetBody(false);
                EnableMonos(true);
                CharacterStatsEventsContainer.OnCharacterStatsAssign.Invoke(_characterStats);
                characterUIHandler.enabled = true;
                characterUIHandler.AssignSingleton();
                _playerEffectsHandler.CanInteract = true;
            }

            StartCoroutine(Destroy());
        }

        private void SetBody(bool value)
        {
            foreach (var part in _body)
                part.enabled = value;
        }

        private void EnableMonos(bool value)
        {
            foreach (var mono in _monos)
                mono.enabled = value;
            foreach (var obj in _disablingObjects)
                obj.SetActive(value);
        }

        private void ClearObjects()
        {
            EnableMonos(false);
            _canvas.SetActive(false);
            _characterStaff.SetActive(false);
        }
    }
}