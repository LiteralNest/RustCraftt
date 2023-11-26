using System.Collections;
using UnityEngine;

public class WarmEffect : MonoBehaviour
{
    [SerializeField] private TemperatureZone _temperatureZone;
    [SerializeField] private float _warmIncreaseInterval = 3f;

    private CharacterStats _characterStats;
    private bool _isEffectActive = false;

    public bool MatchesTrigger(Collider other) => other.CompareTag("WarmEnvironment");

    public void ActivateEffect()
    {
        _isEffectActive = true;
    }
    
    public void SetCharacterStats(CharacterStats characterStats)
    {
        _characterStats = characterStats;
    }

    public void OnEnter(Transform playerPosition)
    {
        Debug.Log("Entered Warm Zone");
        ActivateEffect();
        StartCoroutine(ApplyEffectCoroutine(playerPosition));
    }

    public void OnStay(Transform playerPosition)
    {
        Debug.Log("Stayed Warm Zone");
        float temperature = _temperatureZone.GetTemperatureAtPosition(playerPosition.position);
        ApplyWarmEffect(temperature);
    }

    public void OnExit(Transform player)
    {
        Debug.Log("Exited Warm Zone");
        _isEffectActive = false;
        StopCoroutine(ApplyEffectCoroutine(player));
    }

    private void ApplyWarmEffect(float temperature)
    {
        if (_isEffectActive)
        {
            Debug.Log("Current Temperature: " + temperature);

            // Adjust the temperature thresholds as needed
            if (temperature > 20f)
            {
                Debug.Log("Healing 2 health due to extreme warmth.");
            }
            else if (temperature > 10f)
            {
                Debug.Log("Healing 1 health due to warmth.");
            }
        }
    }
    
    private IEnumerator ApplyEffectCoroutine(Transform player)
    {
        while (_isEffectActive)
        {
            OnStay(player);
            yield return new WaitForSeconds(_warmIncreaseInterval);
        }
    }
}