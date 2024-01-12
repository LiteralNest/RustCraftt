using Events;
using UnityEngine;

namespace InHandItems
{
    public class BuilldingHammer : MonoBehaviour
    {
        private void OnEnable()
            => GlobalEventsContainer.BuildingHammerActivated?.Invoke(true);

        private void OnDisable()
        {
            GlobalEventsContainer.BuildingHammerActivated?.Invoke(false);
   
        }
    }
}