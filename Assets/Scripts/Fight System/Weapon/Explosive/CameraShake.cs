using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _originalPosition;

    private float _shakeDuration;
    private float _shakeMagnitude;
    private float _dampingSpeed = 1.0f;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (_shakeDuration > 0)
        {
            _transform.localPosition = _originalPosition + Random.insideUnitSphere * _shakeMagnitude;
            _shakeDuration -= Time.deltaTime * _dampingSpeed;
        }
        else
        {
            _shakeDuration = 0f;
            _transform.localPosition = _originalPosition;
        }
    }

    public void StartShake(float duration, float magnitude)
    {
        _originalPosition = _transform.localPosition;
        _shakeDuration = duration;
        _shakeMagnitude = magnitude;
    }
}