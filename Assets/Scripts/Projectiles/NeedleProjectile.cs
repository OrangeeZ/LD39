using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleProjectile : Projectile
{
    protected override void Release(Collider other)
    {       
        GetComponent<Collider>().isTrigger = false;
        enabled = false;
    }
}