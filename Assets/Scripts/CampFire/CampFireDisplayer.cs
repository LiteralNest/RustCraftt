using UnityEngine;

public class CampFireDisplayer : MonoBehaviour
{
    [Header("Attached Scripts")] [SerializeField]
    private SlotsDisplayer _slotsDisplayer;
    
    [Header("UI")] 
    [SerializeField] private GameObject _turnOnButton;
    [SerializeField] private GameObject _turnOffButton;

    public void DisplayButton(bool value)
    {
        _turnOnButton.SetActive(!value);
        _turnOffButton.SetActive(value);
    }

    public void SetFlaming(bool value)
    {
        var campfire = _slotsDisplayer.TargetStorage as CampFireHandler;
        campfire.TurnFlamingServerRpc(value);
        DisplayButton(campfire.Flaming.Value);
    }
}