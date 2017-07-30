using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleProjectile : Projectile
{
    public override void OnHit(Collider other)
    {
        if (other != null && other.CompareTag("Environment"))
        {
            GetComponent<Collider>().isTrigger = false;
            enabled = false;
        }
        else
        {
            base.OnHit(other);
        }
    }
}