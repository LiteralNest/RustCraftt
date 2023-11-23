using System.Collections;
using UnityEngine;

public class ColdEffect : MonoBehaviour
{
    [SerializeField] private TemperatureZone _temperatureZone;
    [SerializeField] private float _coldDecreaseInterval = 3f;

    private CharacterStats _characterStats;
    private bool _isEffectActive = false;

    public bool MatchesTrigger(Collider other) => other.CompareTag("ColdEnvironment");

    private void ActivateEffect()
    {
        _isEffectActive = true;
    }
    
    public void SetCharacterStats(CharacterStats characterStats)
    {
        _characterStats = characterStats;
    }

    public void OnEnter(Transform playerPosition,float resist)
    {
        Debug.Log("Entered Cold Zone");
        ActivateEffect();
        StartCoroutine(ApplyEffectCoroutine(playerPosition, resist));
    }

    private void OnStay(Transform playerPosition, float resist)
    {
        Debug.Log("Stayed Cold Zone");
        float temperature = _temperatureZone.GetTemperatureAtPosition(playerPosition.position);
        ApplyColdEffect(temperature, resist);
    }

    public void OnExit(Transform player, float resist)
    {
        Debug.Log("Exited Cold Zone");
        _isEffectActive = false;
        StopCoroutine(ApplyEffectCoroutine(player, resist));
    }

    private void ApplyColdEffect(float temperature, float resist)
    {
        if (_isEffectActive)
        {
            Debug.Log("Current Temperature: " + temperature);

            if (temperature < -15f)
            {
                GlobalEventsContainer.CriticalTemperatureReached?.Invoke();
                DealDamage(4f, resist);
                Debug.Log("Dealing 2 damage due to extreme cold.");
            }
            else if (temperature < -10f)
            {
                DealDamage(2f, resist);
                Debug.Log("Dealing 1 damage due to cold.");
            }
        }
    }


    private void DealDamage(float damageAmount, float resist)
    {
        if (_characterStats != null)
        {
            _characterStats.MinusStat(CharacterStatType.Health, damageAmount * resist);
        }
    }

    private IEnumerator ApplyEffectCoroutine(Transform player, float resist)
    {
        while (_isEffectActive)
        {
            OnStay(player, resist);
            yield return new WaitForSeconds(_coldDecreaseInterval);
        }
    }
}
