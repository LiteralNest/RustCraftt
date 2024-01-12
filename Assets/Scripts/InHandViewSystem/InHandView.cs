using UnityEngine;

namespace InHandViewSystem
{
    public abstract class InHandView : MonoBehaviour
    {
        public abstract void Init(IViewable weapon);
    }
}