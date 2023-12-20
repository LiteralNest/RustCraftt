using Player_Controller;
using UI;
using UnityEngine;

public class ResourceGatheringObject : MonoBehaviour
{
    public AnimationClip GatheringAnimation => _gatheringAnimation;
    [SerializeField] private AnimationClip _gatheringAnimation;
    [SerializeField] private bool _canAttack = true;
    
    private void OnEnable()
    {
        if(_canAttack)
            CharacterUIHandler.singleton.ActivateGatherButton(true);
        GlobalEventsContainer.ResourceGatheringObjectAssign?.Invoke(this);
    }

    public void SetGathering(bool value)
    {
        PlayerNetCode.Singleton.InHandObjectsContainer.SetAttackAnimationServerRpc(value);
    }
}