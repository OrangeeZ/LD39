using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleSphereCollider : MonoBehaviour {

	public float radius = 1f;

	private Vector3 _previousPosition;

	public Vector3 CalculatePunishingForce( SimpleSphereCollider otherCollider ) {

		var deltaVector = otherCollider.transform.position - transform.position;
		var maxDistance = otherCollider.radius + radius;

		if ( deltaVector.x > maxDistance || deltaVector.y > maxDistance || deltaVector.z > maxDistance ) {

			return Vector3.zero;
		}

		var intersectionAmount = deltaVector.magnitude;

		if ( intersectionAmount > maxDistance ) {

			return Vector3.zero;
		}

		if ( intersectionAmount == 0f ) {

			intersectionAmount = Random.Range( -Time.deltaTime, Time.deltaTime );
		}

		var intersectionDirection = deltaVector / intersectionAmount;

		return intersectionDirection * ( maxDistance - intersectionAmount );
	}

	public bool Intersects( SimpleSphereCollider otherCollider ) {

		if ( !otherCollider.enabled ) {

			return false;
		}

		return IntersectsInternal( transform.position, radius, otherCollider.transform.position, otherCollider.radius );
	}

	public bool Intersects( Vector3 otherPosition, float otherRadius ) {

		return IntersectsInternal( transform.position, radius, otherPosition, otherRadius );
	}

	public SimpleSphereCollider IntersectsContinuous( IEnumerable<SimpleSphereCollider> otherColliders ) {

		var from = _previousPosition;
		var to = transform.position;

		var interpolationStep = radius * 0.5f;
		var steps = Mathf.CeilToInt( ( to - from ).magnitude / interpolationStep );

		steps = Mathf.Max( steps, 1 );

		var currentPosition = from;

		for ( var i = 0; i < steps; i++ ) {

			foreach ( var each in otherColliders.Where( each => each.enabled ) ) {

				if ( IntersectsInternal( currentPosition, radius, each.transform.position, each.radius ) ) {

					return each;
				}
			}

			currentPosition = Vector3.MoveTowards( currentPosition, to, interpolationStep );
		}

		return null;
	}

	public SimpleCapsuleCollider IntersectsContinuous( IEnumerable<SimpleCapsuleCollider> otherColliders ) {

		var from = _previousPosition;
		var to = transform.position;

		var interpolationStep = radius * 0.5f;
		var steps = Mathf.CeilToInt( ( to - from ).magnitude / interpolationStep );

		steps = Mathf.Max( steps, 1 );

		var currentPosition = from;

		for ( var i = 0; i < steps; i++ ) {

			foreach ( var each in otherColliders.Where( each => each.enabled ) ) {

				if ( each.Intersects( this ) ) {

					return each;
				}
			}

			currentPosition = Vector3.MoveTowards( currentPosition, to, interpolationStep );
		}

		return null;
	}

	public IEnumerable<SimpleSphereCollider> IntersectsMany( IEnumerable<SimpleSphereCollider> otherColliders ) {

		return otherColliders.Where( each => each != null ).Where( Intersects );
	}

	private bool IntersectsInternal( Vector3 thisPosition, float thisRadius, Vector3 otherPosition, float otherRadius ) {

		return ( thisPosition - otherPosition ).sqrMagnitude <= ( Mathf.Pow( thisRadius + otherRadius, 2 ) );
	}

	private void LateUpdate() {

		_previousPosition = transform.position;
	}

	private void OnDrawGizmos() {

		var from = _previousPosition;
		var to = transform.position;

		var interpolationStep = radius * 0.5f;
		var steps = Mathf.CeilToInt( ( to - from ).magnitude / interpolationStep );

		steps = Mathf.Max( steps, 1 );

		var currentPosition = from;

		for ( var i = 0; i < steps; i++ ) {

			Debug.DrawLine( currentPosition, ( currentPosition = Vector3.MoveTowards( currentPosition, to, interpolationStep ) ) * 0.9f );
		}

		Gizmos.DrawWireSphere( transform.position, radius );
		//Gizmos.DrawRay( transform.position, normal );
	}

}