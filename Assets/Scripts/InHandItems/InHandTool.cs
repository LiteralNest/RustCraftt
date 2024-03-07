using UnityEngine;

namespace InHandItems
{
    public class InHandTool : MonoBehaviour
    {
        [SerializeField] private ResourceGatheringObject _gatheringObject;

        public void Gather()
        {
            if (_gatheringObject.gameObject.activeSelf)
                _gatheringObject.Gather();
        }
    }
}