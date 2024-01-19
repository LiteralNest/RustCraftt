using System.Collections;
using UnityEngine;

namespace DayTime.Phases
{
    [System.Serializable]
    public class DecreaseLightPhase : TimePhase
    {
        public override IEnumerator RunPhaseRoutine(SkyBoxView skyBoxView, float timeScale)
        {
            float currentRange = LightRange.y;
            while (currentRange > LightRange.x)
            {
                currentRange -= Time.deltaTime * timeScale;
                skyBoxView.ExposureAmount.Value = currentRange;
                skyBoxView.LightIntensity.Value = Mathf.Lerp(LightRange.x, LightRange.y, currentRange);
                yield return null;
            }
        }
    }
}