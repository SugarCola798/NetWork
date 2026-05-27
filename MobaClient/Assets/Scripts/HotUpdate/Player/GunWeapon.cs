using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private Transform bulletEffectTransform;
    [SerializeField]
    private float bornInterval;

    private float lastFireTime;

    public void Fire(Vector3 targetPosition)
    {
        if(Time.time - lastFireTime < bornInterval)
        {
            return;
        }
        lastFireTime = Time.time;
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        var direction = (targetPosition - bulletSpawnPoint.position).normalized;
        bullet.Initialize(null, PlayerController.Instance.gameObject, bulletSpawnPoint.position, direction);
    }

    public void PlayHitEffect(Vector3 hitPosition)
    {
        var effect = Instantiate(bulletEffectTransform, hitPosition, Quaternion.identity);
        Destroy(effect.gameObject, 3f);
    }
}
