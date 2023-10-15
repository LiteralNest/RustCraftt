using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private GameObject _uiPanel;
    private bool _playerNearby = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = true;
            ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = false;
            HideUI();
        }
    }

    private void ShowUI()
    {
        _uiPanel.SetActive(true);
    }

    private void HideUI()
    {
        _uiPanel.SetActive(false);
    }
    
}