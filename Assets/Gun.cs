using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float fireRate = 1f;
    float nextFire = 0f;
    public void Fire()
    {
        if (Time.time < nextFire) return;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        nextFire = Time.time + fireRate;

    }
}
