using System.Collections.Generic;
using Building_System.Upgrading;
using Player_Controller;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerNetworkController : MonoBehaviour
{
    [Header("NetCode")] [SerializeField] private PlayerNetCode _playerNetCode;

    [Header("Scripts")] [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerJumper _playerJumper;
    [SerializeField] private PlayerSitter _playerSitter;
    [SerializeField] private PlayerResourcesGatherer _playerResourcesGatherer;
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;
    [SerializeField] private BuildingChooser _buildingChooser;
    [SerializeField] private BuildingDragger _buildingDragger;
    [SerializeField] private PlayerFightHandler _playerFightHandler;
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioListener _listener;
    [SerializeField] private MainUiHandler _mainUiHandler;

    [FormerlySerializedAs("_disablingObjects")] [Header("Children")] [SerializeField]
    private List<GameObject> _body = new List<GameObject>();

    [SerializeField] private GameObject _characterStaff;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _vivox;

    private void Start()
    {
        _vivox.transform.SetParent(null);
        _canvas.transform.SetParent(null);
        if (!_playerNetCode.PlayerIsOwner())
            ClearObjects();
        else
        {
            DisableBody();
            _mainUiHandler.AssignSingleton();
        }
        Destroy(this);
    }

    private void DisableBody()
    {
        foreach (var part in _body)
            part.SetActive(false);
    }

    private void ClearObjects()
    {
        Destroy(_playerController);
        Destroy(_playerJumper);
        Destroy(_playerSitter);
        Destroy(_playerResourcesGatherer);
        Destroy(_objectsRayCaster);
        Destroy(_buildingChooser);
        Destroy(_buildingDragger);
        Destroy(_playerFightHandler);
        _camera.enabled = false;
        _listener.enabled = false;
        Destroy(_canvas);
        Destroy(_vivox);
        Destroy(_characterStaff);
    }
}