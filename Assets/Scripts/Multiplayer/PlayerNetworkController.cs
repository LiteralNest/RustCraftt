using System.Collections.Generic;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using Web.User;

public class PlayerNetworkController : MonoBehaviour
{
    [Header("NetCode")] 
    [SerializeField] private PlayerNetCode _playerNetCode;
    [SerializeField] private List<Behaviour> _monos = new List<Behaviour>();
    [SerializeField] private MainUiHandler _mainUiHandler;
    
    [Header("Children")] 
    [SerializeField] private List<Renderer> _body = new List<Renderer>();
    [SerializeField] private GameObject _characterStaff;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _vivox;

    private void Awake()
    {
        foreach (var mono in _monos)
            mono.enabled = false;
        _mainUiHandler.enabled = false;
        _canvas.SetActive(false);
        _vivox.SetActive(false);
    }
    
    public void Start()
    {
        _canvas.SetActive(true);
        _vivox.SetActive(true);
        _vivox.transform.SetParent(null);
        _canvas.transform.SetParent(null);
        if (!_playerNetCode.IsOwner)
        {
            EnableMonos(false);
            ClearObjects();
        }
        else
        {
            SetBody(false);
            EnableMonos(true);
            _mainUiHandler.AssignSingleton();
        }

        Destroy(this);
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
        Destroy(_canvas);
        Destroy(_vivox);
        Destroy(_characterStaff);
    }
}