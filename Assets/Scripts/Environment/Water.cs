using System.Collections;
using CharacterStatsSystem;
using DamageSystem;
using FightSystem.Damage;
using Player_Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Vehicle;
using Vehicle.Boat;

namespace Environment
{
    public class Water : NetworkBehaviour
    {
        [SerializeField] private GameObject _waterUI;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private UniversalRendererData _data;
        [SerializeField] private string _targetRenderFeature;

        private float _cachedMaxCameraDistance;

        private float _waveHeight = 0f;
        private bool _isRestoringOxygen = false;
        private Coroutine _oxygenCoroutine;

        public bool _inWater;

        private CharacterStats _characterStats;

        private void OnEnable()
        {
            CharacterStatsEventsContainer.OnCharacterStatsAssign += Init;
        }

        private void Init(CharacterStats characterStats)
        {
            _characterStats = characterStats;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.GetComponent<DamagableBodyPart>().IsOwner)
            {
                _inWater = true;
                
                _cachedMaxCameraDistance = Camera.main.farClipPlane;
                Camera.main.farClipPlane = 1000f;
                foreach (var feature in _data.rendererFeatures)
                {
                    if (feature.name == _targetRenderFeature)
                        feature.SetActive(true);
                }

                _mixer.SetFloat("ReverbAmount", 0.5f);
                if (_source)
                    _source.Play();

                _isRestoringOxygen = false;
                if (_waterUI)
                    _waterUI.SetActive(true);
                _oxygenCoroutine = StartCoroutine(RemoveOxygenOverTime());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.GetComponent<DamagableBodyPart>().IsOwner)
            {
                _inWater = false;
                
                foreach (var feature in _data.rendererFeatures)
                {
                    if (feature.name == _targetRenderFeature)
                        feature.SetActive(false);
                }

                Camera.main.farClipPlane = _cachedMaxCameraDistance;

                _mixer.SetFloat("ReverbAmount", 0f);
                if (_source)
                    _source.Stop();

                _isRestoringOxygen = true;
                if (_oxygenCoroutine != null)
                {
                    if (_waterUI)
                        _waterUI.SetActive(false);
                    StopCoroutine(_oxygenCoroutine);
                }

                StartCoroutine(RestoreOxygenToFull());
            }

            if (other.CompareTag("Boat"))
            {
                var boat = other.GetComponent<Boat>();
                if (boat != null)
                {
                    boat.Float(_waveHeight, false);
                }
            }
        }

        private IEnumerator RemoveOxygenOverTime()
        {
            while (!_isRestoringOxygen)
            {
                yield return new WaitForSeconds(0.25f);
                CharacterStatsEventsContainer.OnCharacterStatRemoved.Invoke(CharacterStatType.Oxygen, 1);
            }
        }

        private IEnumerator RestoreOxygenToFull()
        {
            while (_characterStats.Oxygen.Value < 100)
            {
                yield return new WaitForSeconds(0.05f);
                CharacterStatsEventsContainer.OnCharacterStatAdded.Invoke(CharacterStatType.Oxygen, 1);
            }
        }
    }
}