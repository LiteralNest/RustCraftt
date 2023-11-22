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

    public void OnEnter(Transform playerPosition)
    {
        Debug.Log("Entered Cold Zone");
        ActivateEffect();
        StartCoroutine(ApplyEffectCoroutine(playerPosition));
    }

    public void OnStay(Transform playerPosition)
    {
        Debug.Log("Stayed Cold Zone");
        Debug.Log("Player Position: " + playerPosition);
        float temperature = _temperatureZone.GetTemperatureAtPosition(playerPosition.position);
        Debug.Log("Temperature: " + temperature);
        ApplyColdEffect(temperature);
    }

    public void OnExit(Transform player)
    {
        Debug.Log("Exited Cold Zone");
        _isEffectActive = false;
        // _playerPosition = Vector3.zero;
        StopCoroutine(ApplyEffectCoroutine(player));
    }

    private void ApplyColdEffect(float temperature)
    {
        if (_isEffectActive)
        {
            if (temperature < -15f)
            {
                DealDamage(2f);
            }
            else if (temperature < -10f)
            {
                DealDamage(1f);
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
}
