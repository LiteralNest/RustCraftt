using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuUI : MonoBehaviour
{
   public void SetQuality(int qualityIndex)
   {
      QualitySettings.SetQualityLevel(qualityIndex);
   }
}
