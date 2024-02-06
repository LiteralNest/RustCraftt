using UnityEngine;

namespace UI
{
   [RequireComponent(typeof(RectTransform))]
   public class SafeAreaSetter : MonoBehaviour
   {
      [SerializeField] private RectTransform _rectTransform;

      private void Start()
      {
         if(_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
      
         var safeArea = Screen.safeArea;
         var anchorMin = safeArea.position;
         var anchorMax = safeArea.position + safeArea.size;
      
         anchorMin.x /= Screen.width;
         anchorMin.y /= Screen.height;
         anchorMax.x /= Screen.width;
         anchorMax.y /= Screen.height;
      
         _rectTransform.anchorMin = anchorMin;
         _rectTransform.anchorMax = anchorMax;
      }
   }
}