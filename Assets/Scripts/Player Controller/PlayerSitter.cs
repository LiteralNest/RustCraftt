using UnityEngine;

public class PlayerSitter : MonoBehaviour
{
    [SerializeField] private Transform _eyes;
    [SerializeField] private float _downingValue;

    private float _cachedYPos;

    public void Sit()
    {
        _cachedYPos = _eyes.position.y;
        _eyes.position -= new Vector3(0, _downingValue, 0);
    }

    public void Stand()
    {
        _eyes.position = new Vector3(_eyes.position.x, _cachedYPos, _eyes.position.z);
    }
}