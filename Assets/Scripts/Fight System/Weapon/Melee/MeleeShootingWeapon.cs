using UnityEngine;

public class MeleeShootingWeapon : BaseShootingWeapon
{
    public override void Attack(bool value)
    {
        GlobalEventsContainer.ShouldHandleAttacking?.Invoke(value);
    }
}
