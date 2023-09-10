using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNetworkController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    
    [Header("Scripts")] [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerJumper _playerJumper;
    [SerializeField] private PlayerSitter _playerSitter;
    [SerializeField] private PlayerResourcesGatherer _playerResourcesGatherer;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private ObjectsRayCaster _objectsRayCaster;
    [SerializeField] private BuildingChooser _buildingChooser;
    [SerializeField] private BuildingDragger _buildingDragger;
    [SerializeField] private BuildingUpgrader _buildingUpgrader;
    [SerializeField] private ObjectPlacer _objectPlacer;
    [SerializeField] private PlayerFightHandler _playerFightHandler;
    [SerializeField] private PlayerHandler _playerHandler;

    [Header("Children")] [SerializeField] private GameObject _eyes;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _groundCheckerObj;
    [SerializeField] private GameObject _characterStatsObj;
    
    private void Start()
    {
        if(!_playerController.PlayerIsOwner())
            ClearObjects();
        Destroy(this);
    }

    private void ClearObjects()
    {
        Destroy(_playerInput);
        _playerController.enabled = false;
        Destroy(_playerJumper);
        Destroy(_playerSitter);
        Destroy(_playerResourcesGatherer);
        Destroy(_groundChecker);
        Destroy(_objectsRayCaster);
        Destroy(_buildingChooser);
        Destroy(_buildingDragger);
        Destroy(_buildingUpgrader);
        Destroy(_objectPlacer);
        Destroy(_playerHandler);
        Destroy(_playerFightHandler);
        
        Destroy(_eyes);
        Destroy(_inventory);
        Destroy(_groundCheckerObj);
        Destroy(_characterStatsObj);
    }
}