using System.Collections;
using UnityEngine;

namespace DayTime.Phases
{
    [System.Serializable]
    public abstract class TimePhase
    {
        protected static readonly int Exposure = Shader.PropertyToID("_Exposure");
        
        [SerializeField] protected Vector2 LightRange = new Vector2(0f, 1f);

        public abstract IEnumerator RunPhaseRoutine(SkyBoxView skyBoxView, float timeScale);

        protected void SetValues(Material skyMaterial, Light directionalLight, float timeScale)
        {
            skyMaterial.SetFloat(Exposure, timeScale);
            directionalLight.intensity = Mathf.Lerp(LightRange.x, LightRange.y, timeScale);
        }
    }
}