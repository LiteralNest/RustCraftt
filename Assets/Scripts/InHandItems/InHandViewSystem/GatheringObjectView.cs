using UI;
using UnityEngine;

namespace InHandItems.InHandViewSystem
{
    public class GatheringObjectView : InHandView
    {
        [SerializeField] private CustomButton _attackButton;
        public override void Init(IViewable weapon)
        {
            var gatheringObject = weapon as ResourceGatheringObject;
            
            _attackButton.PointerDown.AddListener(() => { gatheringObject.SetGathering(true); });
            _attackButton.PointerClickedWithoudDisable.AddListener(() => { gatheringObject.SetGathering(false); });
            ActiveAttackButton(true);
        }

        private void ActiveAttackButton(bool value) => _attackButton.gameObject.SetActive(value);
    }
}