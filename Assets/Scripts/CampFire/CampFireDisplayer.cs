using UnityEngine;

public class CampFireDisplayer : MonoBehaviour
{
    [Header("UI")] [SerializeField] private GameObject _turnOnButton;
    [SerializeField] private GameObject _turnOffButton;

    [Header("Main Params")] [SerializeField]
    private CampFireSlotsContainer _campFireSlotsContainer;

    public void DisplayButton(bool value)
    {
        _turnOnButton.SetActive(!value);
        _turnOffButton.SetActive(value);
    }

    public void SetFlaming(bool value)
    {
        _campFireSlotsContainer.CampFireHandler.TurnFlamingServerRpc(value);
        DisplayButton(_campFireSlotsContainer.CampFireHandler.Flaming.Value);
    }
}