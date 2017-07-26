using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class SimpleCapsuleCollider : MonoBehaviour {

	public Vector3 center;
	public Vector3 normal;
	public float height;
	public float radius;

	public Object userData;

	public SimpleSphereCollider Intersects( IEnumerable<SimpleSphereCollider> sphereColliders ) {

		return sphereColliders.FirstOrDefault( Intersects );
	}

	public IEnumerable<SimpleSphereCollider> IntersectsMany( IEnumerable<SimpleSphereCollider> sphereColliders ) {

		return sphereColliders.Where( Intersects );
	}

	public bool Intersects( SimpleSphereCollider sphereCollider ) {

		if ( sphereCollider == null || !sphereCollider.enabled ) {

			return false;
		}

		var halfHeight = height * 0.5f;

		var from = center + transform.position - normal * halfHeight;
		var to = from + 2f * ( normal * halfHeight );

		var projectionDistanceNormalized = Vector3.Dot( ( to - from ) / height, sphereCollider.transform.position - from ) / height;

		var projectionPoint = Vector3.Lerp( from, to, projectionDistanceNormalized );

		return IntersectsInternal( projectionPoint, radius, sphereCollider.transform.position, sphereCollider.radius );
	}

	private bool IntersectsInternal( Vector3 thisPosition, float thisRadius, Vector3 otherPosition, float otherRadius ) {

		return ( thisPosition - otherPosition ).sqrMagnitude <= ( Mathf.Pow( thisRadius + otherRadius, 2 ) );
	}

	void OnDrawGizmos() {

		var halfHeight = height * 0.5f;

		var from = center + transform.position - normal * halfHeight;
		var to = from + 2f * ( normal * halfHeight );

		Gizmos.color = Color.green;

		for ( var i = 0f; i <= 1f; i += 0.1f ) {

			Gizmos.DrawWireSphere( Vector3.Lerp( from, to, i ), radius );
		}

		Gizmos.DrawLine( from, to );

		//Gizmos.DrawWireCube( center, extents * 2f );
	}
}
