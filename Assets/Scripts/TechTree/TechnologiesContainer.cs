using System.Collections.Generic;
using UnityEngine;

namespace TechTree
{
    public class TechnologiesContainer : MonoBehaviour
    {
        [SerializeField] private List<TechnologyUI> _technologies = new List<TechnologyUI>();

        public void DeselectTechnologies()
        { 
            foreach(var tech in _technologies)
                tech.Select(false);
        }
    }
}
