using UnityEngine;

public class PlayerClimber : MonoBehaviour
{
    [SerializeField] private float _climbForce = 10.0f;

    private bool _canClimb;

    public void TryClimb(Transform player)
    {
        if (!_canClimb) return;
        player.position += new Vector3(0,_climbForce * Time.deltaTime,0);
        return;
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (!other.CompareTag("Ladder")) return;
        _canClimb = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Ladder")) return;
        _canClimb = false;
    }
}