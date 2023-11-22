using System.Collections;
using UnityEngine;

public class ColdEffect : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private TemperatureZone _temperatureZone;
    [SerializeField] private float _coldDecreaseInterval = 3f;

    private CharacterStats _characterStats;
    private bool _isEffectActive = false;

    public bool MatchesTrigger(Collider other) => other.CompareTag("ColdEnvironment");

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
        Debug.Log("Entered Cold Zone");
        ActivateEffect();
        StartCoroutine(ApplyEffectCoroutine(playerPosition));
    }

    private void OnStay(Transform playerPosition)
    {
        Debug.Log("Stayed Cold Zone");
        float temperature = _temperatureZone.GetTemperatureAtPosition(playerPosition.position);
        ApplyColdEffect(temperature);
    }

    public void OnExit(Transform player)
    {
        Debug.Log("Exited Cold Zone");
        _isEffectActive = false;
        StopCoroutine(ApplyEffectCoroutine(player));
    }

    private void ApplyColdEffect(float temperature)
    {
        if (_isEffectActive)
        {
            Debug.Log("Current Temperature: " + temperature);

            if (temperature < -15f)
            {
                DealDamage(2f);
                Debug.Log("Dealing 2 damage due to extreme cold.");
            }
            else if (temperature < -10f)
            {
                DealDamage(1f);
                Debug.Log("Dealing 1 damage due to cold.");
            }
        }
    }


    private void DealDamage(float damageAmount)
    {
        if (_characterStats != null)
        {
            _characterStats.MinusStat(CharacterStatType.Health, damageAmount);
        }
    }

    private IEnumerator ApplyEffectCoroutine(Transform player)
    {
        while (_isEffectActive)
        {
            OnStay(player);
            yield return new WaitForSeconds(_coldDecreaseInterval);
        }
    }


    public float GetColdZoneRadius()
    {
        if (_sphereCollider != null)
        {
            return _sphereCollider.radius;
        }

        return 0f;
    }
}
