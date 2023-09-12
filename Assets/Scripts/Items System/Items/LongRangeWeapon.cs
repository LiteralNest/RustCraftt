using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon")]
[System.Serializable]
public class LongRangeWeapon : Weapon
{
    [field:SerializeField] public float FirePower { get; private set; }
    [field:SerializeField] public Ammo Ammo { get; private set; }
    [field:SerializeField] public float Spread { get; private set; }
    [field:SerializeField] public int MagazineAmmoCount { get; private set; }
    [field:SerializeField] public float ReloadingTime { get; private set; }
    [field:SerializeField] public float DelayBetweenShoots { get; private set; }
    
    public void AssignShoot(GameObject target, Vector3 directionSpread)
    {
        target.transform.SetParent(null);
        target.transform.forward = directionSpread.normalized;

        target.GetComponent<Rigidbody>().AddForce(directionSpread.normalized * FirePower, ForceMode.Impulse);
        target.transform.eulerAngles += new Vector3(0, -90, 0);
    }
}
