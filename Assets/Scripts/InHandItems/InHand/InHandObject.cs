using System.Collections.Generic;
using UnityEngine;

namespace InHandItems.InHand
{
    public class InHandObject : MonoBehaviour
    {
        [Header("Start Init")] 
        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private List<GameObject> _activatingObjects;
        
        public virtual void Walk(bool value){}
        public virtual void Run(bool value){}
        public virtual void HandleAttacking(bool attack){}

        public void DisplayRenderers(bool value)
        {
            foreach(var renderer in _renderers)
                renderer.enabled = value;
            foreach(var activatingObject in _activatingObjects)
                activatingObject.SetActive(value);
        }
    }
}