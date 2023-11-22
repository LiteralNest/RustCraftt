using UnityEngine;

public class TemperatureZone : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    public float temperatureStep = 5f;
    public float maxTemperature = -25f;
    public float minTemperature = 0f;
    public float alpha = 0.5f;
    public bool showTemperatures = true;

    public float GetTemperatureAtPosition(Vector3 position)
    {
        var distance = Vector3.Distance(position, transform.position);
        
        return Mathf.Lerp(minTemperature, maxTemperature, NormalizeBetweenZeroAndOne(distance));
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
        if (!showTemperatures)
            return;
        
        
        var maxRadius = _sphereCollider.radius;
        
        var numberOfSpheres = Mathf.CeilToInt(maxRadius / temperatureStep);
        
        float step = maxRadius / numberOfSpheres;

        for (var i = 0; i < numberOfSpheres; i++)
        {
            var currentRadius = maxRadius - i * step;
            
            var normalizedDistance = currentRadius / maxRadius;
            
            var sphereColor = Color.Lerp(Color.blue, Color.red, normalizedDistance);
            
            sphereColor.a = alpha;
            
            Gizmos.color = sphereColor;

            var lerp = Mathf.Lerp(minTemperature, maxTemperature, normalizedDistance);


            Gizmos.DrawSphere(transform.position, currentRadius);
        }
    }
}
