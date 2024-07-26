using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Effect
{
    Projectile thisProjectile;

    [SerializeField] float radius;

    private void Start()
    {
        thisProjectile = this.transform.GetComponent<Projectile>();
        PlaySFX(instantiateSFXName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Target"))
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius, ~0, QueryTriggerInteraction.UseGlobal);
            for (int i = 0; i < colliders.Length; i++)
            {
            }

            if (particleTransform)
            {
                GameObject.Instantiate(particleTransform, this.transform.position, this.transform.rotation);
            }

            PlaySFX(destroySFXName);
            Destroy(this.transform.gameObject);
        }
    }
}
