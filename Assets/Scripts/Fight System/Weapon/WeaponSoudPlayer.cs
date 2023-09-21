using UnityEngine;

public class WeaponSoudPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _shotClip;
    [SerializeField] private AudioSource _shotSource;

    public void PlayShot()
        => _shotSource.PlayOneShot(_shotClip);
}