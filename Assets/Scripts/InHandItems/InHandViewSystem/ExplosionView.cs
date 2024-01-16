using UnityEngine;
using UnityEngine.UI;

namespace InHandItems.InHandViewSystem
{
    public class ExplosionView : InHandView
    {
        [SerializeField] private Button _throwButton;
        
        public override void Init(IViewable weapon)
        {
            var explosionWeapon = weapon as InHandExplosive;
            _throwButton.onClick.AddListener(() => explosionWeapon.TryThrow());
            _throwButton.gameObject.SetActive(true);
        }
    }
}