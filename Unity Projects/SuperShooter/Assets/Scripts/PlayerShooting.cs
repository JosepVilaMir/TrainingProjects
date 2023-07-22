using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shootPoint;
    [SerializeField] private ParticleSystem muzzleEffect;
    [SerializeField] private AudioSource shootSound;

    public void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            GameObject clone = Instantiate(bulletPrefab);

            clone.transform.position = shootPoint.transform.position;
            clone.transform.rotation = shootPoint.transform.rotation;

            muzzleEffect.Play();
            shootSound.Play();
        }
    }

    // Update is called once per frame
    public void OnFire(Input value)
    {
        Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
    }
}
