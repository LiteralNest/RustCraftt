using System;
using System.Collections;
using Character_Stats;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.DeathScreen
{
    public class KnockDownScreenUI : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;
        [Header("")]
        [SerializeField] private int _deathTimer = 30;
        [SerializeField] private TextMeshProUGUI _deathTimerText;
        [SerializeField] private Image _deathProgressBar;
        [Header("")]
        [SerializeField] private int _reviveChance = 20;
        [SerializeField] private TextMeshProUGUI _reviveChanceText;
        [SerializeField] private Image _reviveProgressBar;
        
        private Vector3 _originalCameraPosition;
        private Quaternion _originalCameraRotation;
        private Transform _transform;
    
        private float _movementSpeed = 1f;

        private int _currentDeathTimer;

        private void Awake()
        {
            _transform = _playerCamera.transform;
            _originalCameraPosition = _transform.localPosition;
            _originalCameraRotation = _transform.localRotation;
         
            gameObject.SetActive(false);
        }

        private void Start()
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
            while (_currentDeathTimer > 0)
            {
                yield return new WaitForSeconds(1);
                _currentDeathTimer--;
                // move camera
                var camTransform = _playerCamera.transform;
                camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, new Vector3(camTransform.localPosition.x, 0f, camTransform.localPosition.z), Time.deltaTime * _movementSpeed);
                camTransform.localRotation = Quaternion.Euler(camTransform.localRotation.eulerAngles.x, camTransform.localRotation.eulerAngles.y, -70);

                // Update timer bar
                    _deathProgressBar.fillAmount = _currentDeathTimer / 30f;
                _deathTimerText.text = _currentDeathTimer.ToString();

                // Update chance bar
                _reviveProgressBar.fillAmount = _reviveChance / 100f;
                _reviveChanceText.text = _reviveChance.ToString();
            }

            var randomChance = Random.Range(0f, 100f);
            Debug.Log("Chance:" + randomChance);
            if (randomChance <= _reviveChance)
            {
                RelivePlayer();
                _currentDeathTimer = _deathTimer; 
            }
            else
            {
                CharacterStats.Singleton.MinusStat(CharacterStatType.Health, 10);
                _currentDeathTimer = _deathTimer; 
                SetCameraOriginPos();
                gameObject.SetActive(false);
            }
        }
        
        //Need to Implement behavior of player knock
        public void RelivePlayer()
        {
            _currentDeathTimer = _deathTimer;

            gameObject.SetActive(false);

            CharacterStats.Singleton.PlusStat(CharacterStatType.Health, 30);
            CharacterStats.Singleton.PlusStat(CharacterStatType.Food, 10);
            CharacterStats.Singleton.PlusStat(CharacterStatType.Water, 10);
            
            SetCameraOriginPos();
        }

        private void SetCameraOriginPos()
        {
            _playerCamera.transform.localPosition = _originalCameraPosition;
            _playerCamera.transform.localRotation = _originalCameraRotation;
        }
    }
}