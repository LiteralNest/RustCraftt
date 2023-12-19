using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Animation_System;
using Multiplayer.PlayerSpawning;
using Player_Controller;
using UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerNetworkController : MonoBehaviour
{
    [Header("NetCode")] [SerializeField] private PlayerNetCode _playerNetCode;
    [SerializeField] private CharacterAnimationsHandler _characterAnimationsHandler;
    [SerializeField] private CharacterAnimationsHandler _inventoryCharacterAnimationsHandler;
    [SerializeField] private List<Behaviour> _monos = new List<Behaviour>();

    [FormerlySerializedAs("_mainUiHandler")] [SerializeField]
    private CharacterUIHandler characterUIHandler;

    [Header("Children")] [SerializeField] private List<Renderer> _body = new List<Renderer>();
    [SerializeField] private GameObject _characterStaff;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _vivox;

    private void Awake()
    {
        foreach (var mono in _monos)
            mono.enabled = false;
        characterUIHandler.enabled = false;
        _canvas.SetActive(false);
        _vivox.SetActive(false);
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

        this.enabled = false;
    }

    public void Start()
    {
        _canvas.SetActive(true);
        _vivox.SetActive(true);
        _vivox.transform.SetParent(null);
        if (!_playerNetCode.IsOwner)
        {
            EnableMonos(false);
            ClearObjects();
        }
        else
        {
            SetBody(false);
            EnableMonos(true);
            characterUIHandler.AssignSingleton();
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
    }

    private void ClearObjects()
    {
        EnableMonos(false);
        _canvas.SetActive(false);
        _vivox.SetActive(false);
        _characterStaff.SetActive(false);
        // Destroy(_canvas);
        // Destroy(_vivox);
        // Destroy(_characterStaff);
    }
}