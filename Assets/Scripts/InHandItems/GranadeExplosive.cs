using InHandItems.InHandAnimations.Weapon;
using InHandItems.InHandViewSystem;
using UnityEngine;

namespace InHandItems
{
    public class GranadeExplosive : InHandExplosive
    {
        private const string ViewName = "Weapon/View/GranadeView";

        [SerializeField] private GranadeAnimator _granadeAnimator;
        

        private void Start()
        {
            var view = Instantiate(Resources.Load<GranadeView>(ViewName), this.transform);
            view.Init(this);
        }

        public void Scope()
            => _granadeAnimator.PlayScope();

        public void Throw()
        {
            _granadeAnimator.PlayThrow();
        }
    }
}