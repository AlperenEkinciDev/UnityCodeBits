using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : Weapon
{
    [Header("Weapon Properties")]
    [SerializeField] Transform projectile;
    [SerializeField] Transform[] exitPositions;
    [SerializeField][Tooltip("Per Second")] float fireRate = 1f;
    [SerializeField][Tooltip("Per Firing")] float volleyCount = 1f;
    [SerializeField][Range(0.0f, 1.0f)] float volleyDurationPercentage = 1f;
    [SerializeField][Tooltip("Per Volley")] float burstCount = 1f;
    [SerializeField] float exitVelocity = 100f;
    [SerializeField][Range(0.0f, 1.0f)] float exitVelocityRandomness = 0f;
    [SerializeField][Range(0.0f, 1.0f)] float directionRandomness = 0f;
    [SerializeField] bool isDirectFire = false;

    private WeaponTargetLock weaponTargetLock;
    private bool isFireOn = true;
    private float lastFireTime = 0f;
    private int currentExitPos = 0;
    private float lastVolleyTime = 0.0f;


    void Start()
    {
        weaponTargetLock = this.transform.GetComponent<WeaponTargetLock>();
    }

    private void Update()
    {
        isFireOn = weaponTargetLock.targetTransform != null;

        CheckFire();
    }

    void Volley()
    {
        StartCoroutine(VolleyCoroutine());
        lastFireTime = Time.time;
    }

    void Fire()
    {
        Burst();
        currentExitPos = (currentExitPos + 1 ) % exitPositions.Length;
    }

    void Burst()
    {
        Transform exitPosition = exitPositions[currentExitPos];

        for (int i = 0; i < burstCount; i++)
        {
            Transform projectileTransform = GameObject.Instantiate(projectile, exitPosition.position, exitPosition.rotation);
            Rigidbody projectileRigidbody = projectileTransform.GetComponent<Rigidbody>();
            Projectile projectileP = projectileTransform.GetComponent<Projectile>();
            projectileP.targetTransform = weaponTargetLock.targetTransform;
            projectileP.spawnPointTransform = exitPosition;
            projectileP.thisRigidbody = projectileRigidbody;

            Vector3 dir = Vector3.zero;
            if (isDirectFire)
            {
                dir = (((weaponTargetLock.targetTransform.position + weaponTargetLock.trackOffset) - exitPosition.position).normalized + new Vector3(Random.Range(-directionRandomness, directionRandomness), Random.Range(-directionRandomness, directionRandomness), Random.Range(-directionRandomness, directionRandomness))).normalized;
            }
            else
            {
                dir = (exitPosition.transform.forward + new Vector3(Random.Range(-directionRandomness, directionRandomness), Random.Range(-directionRandomness, directionRandomness), Random.Range(-directionRandomness, directionRandomness))).normalized;
            }
            projectileRigidbody.velocity = dir * exitVelocity + dir * exitVelocity * Random.Range(-exitVelocityRandomness, exitVelocityRandomness);
        }
    }

    void CheckFire()
    {
        if (isFireOn)
        {
            bool isOnCooldown = (Time.time - lastFireTime) < (1 / fireRate);

            if (!isOnCooldown)
            {
                Volley();
            }
        }
    }

    IEnumerator VolleyCoroutine()
    {
        if (Mathf.Approximately(volleyDurationPercentage, 0.0f))
        {
            for (int i = 0; i < volleyCount; i++)
            {
                Fire();
            }

            yield return new WaitForEndOfFrame();
        }
        else
        {
            for (int i = 0; i < volleyCount; i++)
            {
                Fire();

                yield return new WaitForSeconds(((1 / fireRate) * volleyDurationPercentage) / volleyCount);
            }
        }
    }
}
