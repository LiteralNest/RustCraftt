using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    private Vector3 currentRotation;

    public void ApplyRecoil(float recoilX, float recoilY, float recoilZ)
    {
        currentRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    public void UpdateRecoil()
    {
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
}