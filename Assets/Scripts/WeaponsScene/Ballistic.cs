using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballistic : Effect
{
    Projectile thisProjectile;

    private void Start()
    {
        thisProjectile = this.transform.GetComponent<Projectile>();
        PlaySFX(instantiateSFXName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (particleTransform)
        {
            Transform particleEffect = GameObject.Instantiate(particleTransform, thisProjectile.targetTransform.position + Vector3.Scale((this.transform.position - thisProjectile.targetTransform.position), this.transform.TransformDirection(new Vector3(0.25f, 1f, -0.1f))), this.transform.rotation);
        }

        PlaySFX(destroySFXName);
        Destroy(this.gameObject);
    }
}
