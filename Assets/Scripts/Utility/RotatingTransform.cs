using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTransform : MonoBehaviour
{
    [SerializeField]
    private float _radius;

    public void RotateByLinearMotion(float linearDistanceDelta)
    {
        var angularDeltaRadians = linearDistanceDelta / (_radius);// * Time.deltaTime;
        transform.rotation *= Quaternion.AngleAxis(angularDeltaRadians * Mathf.Rad2Deg, Vector3.forward);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}