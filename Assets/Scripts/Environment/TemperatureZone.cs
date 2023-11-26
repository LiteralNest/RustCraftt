using UnityEngine;
using UnityEngine.Serialization;

public class TemperatureZone : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private float _temperatureStep = 5f;
    [SerializeField] private float _minTemperature = -25f;
    [SerializeField] private float _maxTemperature = 0f;

    [Header("Draw in Editor")]
    [SerializeField] private bool _showTemperatures = true;
    [SerializeField] private float _alpha = 0.5f;
    [SerializeField] private Color _minTemperatureColor = Color.blue;
    [SerializeField] private Color _maxTemperatureColor = Color.red;

    public float GetTemperatureAtPosition(Vector3 position)
    {
        var distance = Vector3.Distance(position, transform.position);

        return Mathf.Lerp(_maxTemperature, _minTemperature, NormalizeBetweenZeroAndOne(distance));
    }

    private float NormalizeBetweenZeroAndOne(float value)
    {
        float minValue = 0.0f;
        float maxValue = _sphereCollider.radius;
        value = Mathf.Clamp(value, minValue, maxValue);
        float normalizedValue = (value - minValue) / (maxValue - minValue);
        return normalizedValue;
    }

    private void OnDrawGizmos()
    {
        if (!_showTemperatures)
            return;

        var maxRadius = _sphereCollider.radius;

        var numberOfSpheres = Mathf.CeilToInt(maxRadius / _temperatureStep);

        float step = maxRadius / numberOfSpheres;

        for (var i = 0; i < numberOfSpheres; i++)
        {
            var currentRadius = maxRadius - i * step;

            var normalizedDistance = currentRadius / maxRadius;

            var sphereColor = Color.Lerp(_minTemperatureColor, _maxTemperatureColor, normalizedDistance);

            sphereColor.a = _alpha;

            Gizmos.color = sphereColor;

            var lerp = Mathf.Lerp(_maxTemperature, _minTemperature, normalizedDistance);

            Gizmos.DrawSphere(transform.position, currentRadius);
        }
    }
}