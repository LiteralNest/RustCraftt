using Building_System;
using Player_Controller;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClimber : MonoBehaviour
{
    [SerializeField] private float _climbForce = 10.0f;
    [SerializeField] private PlayerJumper _jumper;
    [SerializeField] private Button climbUIButton;
    [SerializeField] private float raycastDistance = 0.3f; 

    private bool _canClimb;
    private Ladder _currentLadder; 

    private void Start()
    {
        climbUIButton.onClick.AddListener(ClimbButtonPressed);
    }

    
    private void ClimbButtonPressed()
    {
        if (_canClimb && _currentLadder != null)
        {
            TryClimb(transform, _currentLadder);
        }
    }

    public void TryClimb(Transform player, Ladder ladder)
    {
        if (!_canClimb) return;

        // Calculate the climb direction (use ladder's up direction)
        Vector3 climbDirection = ladder.transform.up;

        // Update the player's position along the climb direction
        player.position += climbDirection * _climbForce * Time.deltaTime;
    
        // Ensure the player's CharacterController is moved as well
        _jumper.MoveWithLadder(climbDirection * _climbForce * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance))
        {
            Ladder ladder = hit.collider.GetComponent<Ladder>();
            if (ladder != null && hit.collider.CompareTag("DamagingItem"))
            {
                climbUIButton.gameObject.SetActive(true);
                _currentLadder = ladder;
            }
            else
            {
                climbUIButton.gameObject.SetActive(false);
                _currentLadder = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Ladder ladder = other.GetComponent<Ladder>();
        if (ladder != null && other.CompareTag("DamagingItem"))
        {
            _canClimb = true;
            _jumper.SetGravity(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ladder ladder = other.GetComponent<Ladder>();
        if (ladder != null && other.CompareTag("DamagingItem"))
        {
            _canClimb = false;
            _jumper.SetGravity(true);
            climbUIButton.gameObject.SetActive(false);
            _currentLadder = null;
        }
    }
}
