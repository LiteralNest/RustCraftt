using UnityEngine;
using UnityEngine.UI;

namespace TechTree
{
    [RequireComponent(typeof(Button))]
    public class TechTreeClose: MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                GlobalValues.CanLookAround = true;
            });
        }
    }
}