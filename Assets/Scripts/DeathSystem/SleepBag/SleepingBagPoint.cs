using UI.DeathScreen;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DeathSystem.SleepBag
{
    public class SleepingBagPoint : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            DeathScreenUI.Singleton.RespawnInCoordinates(transform.position);
        }
    }
}
