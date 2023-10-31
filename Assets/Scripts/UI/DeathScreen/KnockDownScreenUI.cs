using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private bool _shouldMoveCamera = true;
        private float _movementSpeed = 1f; //
        private void Start()
        {
            _deathProgressBar.fillAmount = 1f;
            _deathTimerText.text = _deathTimer.ToString();

            _reviveProgressBar.fillAmount = 1f;
            _reviveChanceText.text = _reviveChance.ToString();

            _transform = _playerCamera.transform;
            _originalCameraPosition = _transform.position;
            _originalCameraRotation = _transform.rotation;

            StartCoroutine(DeathTimerCoroutine());
        }
        
        private void Update()
        {
            if (!_shouldMoveCamera) return;
            _playerCamera.transform.position = Vector3.Lerp(_playerCamera.transform.position, new Vector3(_playerCamera.transform.position.x,0f,_playerCamera.transform.position.z), Time.deltaTime * _movementSpeed);
            _playerCamera.transform.rotation = Quaternion.Euler(_playerCamera.transform.rotation.eulerAngles.x, _playerCamera.transform.rotation.eulerAngles.y, -70);

           
            if (Vector3.Distance(_playerCamera.transform.position, _originalCameraPosition) < 0.01f) 
                _shouldMoveCamera = false;
            
        }
        private IEnumerator DeathTimerCoroutine()
        {
            while (_deathTimer > 0)
            {
                yield return new WaitForSeconds(1);
                _deathTimer--;

                // Update timer bar
                _deathProgressBar.fillAmount = _deathTimer / 30f;
                _deathTimerText.text = _deathTimer.ToString();

                // Update chance bar
                _reviveProgressBar.fillAmount = _reviveChance / 100f;
                _reviveChanceText.text = _reviveChance.ToString();
            }

            var randomChance = Random.Range(0f, 100f);
            Debug.Log("Chance:" + randomChance);
            if (randomChance <= _reviveChance)
            {
                RevivePlayer();
            }
            else
            {
                CharacterStats.Singleton.MinusStat(CharacterStatType.Health, 10);
                gameObject.SetActive(false);
            }
        }

        
        //Need to Implement behavior of player knock
        private void RevivePlayer()
        {
            _shouldMoveCamera = true;
            gameObject.SetActive(false);
            
            CharacterStats.Singleton.PlusStat(CharacterStatType.Food, 10);
            CharacterStats.Singleton.PlusStat(CharacterStatType.Water, 10);
            
            _playerCamera.transform.position = _originalCameraPosition;
            _playerCamera.transform.rotation = _originalCameraRotation;
        }
    }
}