using UnityEngine;

public class RecyclerDisplayer : MonoBehaviour
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

    public void SetTurned(bool value)
    {
        var recycler = _slotsDisplayer.TargetStorage as Recycler;
        recycler.SetTurnedServerRpc(value);
        DisplayButton(recycler.Turned.Value);
    }
}
