using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aoiti.Techtrees
{
    
    [CreateAssetMenu(menuName = "Techtrees/new Tech")]
    // [DisallowMultipleComponent]
    public class Tech : ScriptableObject
    {
        [TextArea(3, 10)]
        public string definition;
        public Texture2D image;
        [SerializeField]
        public UnityEvent OnResearchComplete= new UnityEvent();
    }

}
