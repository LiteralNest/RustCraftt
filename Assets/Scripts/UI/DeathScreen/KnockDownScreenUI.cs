using System.Collections;
using PlayerDeathSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Web.UserData;
using Random = UnityEngine.Random;

namespace UI.DeathScreen
{
    public class KnockDownScreenUI : MonoBehaviour
    {
        [Header("")] [SerializeField] private int _deathTimer = 20;
        [SerializeField] private TMP_Text _deathTimerText;
        [SerializeField] private Image _deathProgressBar;
        [Header("")] [SerializeField] private int _reviveChance = 20;
        [SerializeField] private TMP_Text _reviveChanceText;
        [SerializeField] private Image _reviveProgressBar;
        private Transform _transform;

        private int _currentDeathTimer;
        
        private void OnEnable()
        {
            _currentDeathTimer = _deathTimer;
            _deathProgressBar.fillAmount = 1f;
            _deathTimerText.text = _currentDeathTimer.ToString();

            _reviveProgressBar.fillAmount = 1f;
            _reviveChanceText.text = _reviveChance.ToString();

            StartCoroutine(DeathTimerCoroutine());
        }

        private IEnumerator DeathTimerCoroutine()
        {
            _reviveProgressBar.fillAmount = _reviveChance / 100f;
            _reviveChanceText.text = _reviveChance.ToString();

            while (_currentDeathTimer > 0)
            {
                yield return new WaitForSeconds(1);
                _currentDeathTimer--;

                _deathProgressBar.fillAmount = _currentDeathTimer / 20f;
                _deathTimerText.text = _currentDeathTimer.ToString();
            }

            var randomChance = Random.Range(0, 100);
            if (randomChance <= _reviveChance)
            {
                PlayerKnockDowner.Singleton.StandUpServerRpc();
                _currentDeathTimer = _deathTimer;
            }
            else
            {
                PlayerKiller.Singleton.DieServerRpc(UserDataHandler.Singleton.UserData.Id, false);
                gameObject.SetActive(false);
            }
        }
    }
}