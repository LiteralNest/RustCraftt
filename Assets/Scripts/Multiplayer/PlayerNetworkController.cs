using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetworkController : MonoBehaviour
{
    [Header("NetCode")] 
    [SerializeField] private PlayerNetCode _playerNetCode;

    [Header("Scripts")] 
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerJumper _playerJumper;
    [SerializeField] private PlayerSitter _playerSitter;
    [SerializeField] private PlayerResourcesGatherer _playerResourcesGatherer;
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;
    [SerializeField] private BuildingChooser _buildingChooser;
    [SerializeField] private BuildingDragger _buildingDragger;
    [SerializeField] private BuildingUpgrader _buildingUpgrader;
    [SerializeField] private PlayerFightHandler _playerFightHandler;

    [Header("Children")] [SerializeField] private GameObject _eyes;
    [SerializeField] private GameObject _characterStaff;
    [SerializeField] private GameObject _canvas;

    private void Start()
    {
        _canvas.transform.SetParent(null);
        if(!_playerNetCode.PlayerIsOwner())
            ClearObjects();
        Destroy(this);
    }

    private void ClearObjects()
    {
        Destroy(_playerInput);
        Destroy(_playerController);
        Destroy(_playerJumper);
        Destroy(_playerSitter);
        Destroy(_playerResourcesGatherer);
        Destroy(_objectsRayCaster);
        Destroy(_buildingChooser);
        Destroy(_buildingDragger);
        Destroy(_buildingUpgrader);
        Destroy(_playerFightHandler);

        Destroy(_eyes);
        Destroy(_canvas);
        Destroy(_characterStaff);
    }
}