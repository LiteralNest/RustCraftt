using UI;
using UnityEngine;

namespace InHandItems.InHandViewSystem
{
    public class GranadeView : InHandView
    {
        [SerializeField] private CustomButton _customButton;

        public override void Init(IViewable weapon)
        {
            var granade = weapon as GranadeExplosive;
            _customButton.PointerDown.AddListener(() => { granade.Scope(); });
            _customButton.PointerClickedWithoudDisable.AddListener(() => { granade.Throw(); });
        }
    }
}