namespace InHandItems.InHandAnimations
{
    public class BuildingHammerAnimator : InHandAnimator
    {
        private const string Build = "Build";
        
        public void SetBuild()
            => PlayAnimationServerRpc(Build);
    }
}