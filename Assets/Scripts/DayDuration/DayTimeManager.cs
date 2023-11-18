using System;
using TMPro;
using UnityEngine;

namespace DayDuration
{
    public class DayTimeManager : MonoBehaviour
    {
        [Header("SourceOfLightning")]
        [SerializeField] private Light _sunLight;
        [SerializeField] private Light _moonLight;
        [Header("TimeOptions")]
        [SerializeField] private float _timeMultiplier;
        [SerializeField] private float _startHour;
        [Header("SetTimeForLightning")]
        [SerializeField] private float _sunriseHour;
        [SerializeField] private float _sunsetHour;
        [Header("AmbientLightningHandlers")]
        [SerializeField] private Color _dayAmbientLighting;
        [SerializeField] private Color _nightAmbientLighting;
        [SerializeField] private AnimationCurve _lightChangeCurve;
        [SerializeField] private float _maxSunlightIntensity;
        [SerializeField] private float _maxMoonlightIntensity;

        private DateTime _currentTime;
        private TimeSpan _sunriseTime;
        private TimeSpan _sunsetTime; 
        private void Start()
        {
            _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_startHour);
            _sunriseTime = TimeSpan.FromHours(_sunriseHour);
            _sunsetTime = TimeSpan.FromHours(_sunsetHour);
        }

        private void Update()
        {
            UpdateTimeOfDay();
            RotateSunAndMoon();
            UpdateLightSettings();
        }

        private void UpdateTimeOfDay()
        {
            _currentTime = _currentTime.AddSeconds(Time.deltaTime * _timeMultiplier);
        }

        private void RotateSunAndMoon()
        {
            float rotateSunLight;
            float rotateMoonLight;

            if(_currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime)
            {
                var sunriseToSunsetDuration = CalculateTimeDifference(_sunriseTime, _sunsetTime);
                var timeSinceSunrise = CalculateTimeDifference(_sunriseTime, _currentTime.TimeOfDay);

                var percent = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

                rotateSunLight = Mathf.Lerp(0, 180, (float)percent);
                rotateMoonLight = Mathf.Lerp(180, 360, (float)percent);
            }
            else
            {
                var sunsetToSunriseDuration = CalculateTimeDifference(_sunsetTime, _sunriseTime);
                var timeSinceSunset = CalculateTimeDifference(_sunsetTime, _currentTime.TimeOfDay);

                var percent = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

                rotateSunLight = Mathf.Lerp(180, 360, (float)percent);
                rotateMoonLight = Mathf.Lerp(0, 180, (float)percent);
            }

            _sunLight.transform.rotation = Quaternion.AngleAxis(rotateSunLight, Vector3.right);
            _moonLight.transform.rotation = Quaternion.AngleAxis(rotateMoonLight, Vector3.right);
        }
        
        private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
        {
            var diff = toTime - fromTime;
            var amounOfHours = 24;
            
            if (diff.TotalSeconds < 0)
            {
                 diff +=  TimeSpan.FromHours(amounOfHours);
            }

            return diff;
        }

        private void UpdateLightSettings()
        {
            var dotProduct = Vector3.Dot(_sunLight.transform.forward, Vector3.down);
            _sunLight.intensity = Mathf.Lerp(0, _maxSunlightIntensity, _lightChangeCurve.Evaluate(dotProduct));
            _moonLight.intensity = Mathf.Lerp(0, _maxMoonlightIntensity, _lightChangeCurve.Evaluate(dotProduct));
            RenderSettings.ambientLight = Color.Lerp(_nightAmbientLighting, _dayAmbientLighting,_lightChangeCurve.Evaluate(dotProduct));
        }
    }
}
