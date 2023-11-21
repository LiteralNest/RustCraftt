using UnityEngine;

public class TemperatureZone : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    public float temperatureStep = 2f;
    public float maxTemperature = 0f;
    public float minTemperature = -16f;
    public float alpha = 0.5f;
    public bool showTemperatures = true;

    public float GetTemperatureAtPosition(Vector3 position)
    {
        var sphereRadius = GetComponent<SphereCollider>().radius;
        var normalizedDistance = Vector3.Distance(position, transform.position) / sphereRadius;
        return Mathf.Lerp(minTemperature, maxTemperature, normalizedDistance);
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
