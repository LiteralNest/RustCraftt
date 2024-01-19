using System.Collections;
using UnityEngine;

namespace DayTime.Phases
{
    [System.Serializable]
    public class IncreaseLightPhase : TimePhase
    {
        public override IEnumerator RunPhaseRoutine(SkyBoxView skyBoxView, float timeScale)
        {
            float currentRange = LightRange.x;
            while (currentRange < LightRange.y)
            {
                currentRange += Time.deltaTime * timeScale;
                skyBoxView.ExposureAmount.Value = currentRange;
                skyBoxView.LightIntensity.Value = Mathf.Lerp(LightRange.x, LightRange.y, currentRange);
                yield return null;
            }
        }
    }
}