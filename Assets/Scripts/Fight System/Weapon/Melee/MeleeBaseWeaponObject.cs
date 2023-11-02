using UnityEngine;

public class MeleeWeaponObject : WeaponObject
{
    public override void Attack(bool value)
    {
        GlobalEventsContainer.ShouldHandleAttacking?.Invoke(value);
    }
}
