using UnityEngine;

public class MeleeWeaponObject : WeaponObject
{
    public override void Attack(bool value)
    {
        InHandObjectsContainer.singleton.HandleAttacking(value);
    }
}
