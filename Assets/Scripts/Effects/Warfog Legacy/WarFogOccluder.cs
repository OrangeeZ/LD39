using UnityEngine;
using System.Collections;

public class WarFogOccluder : MonoBehaviour {

	[SerializeField]
	private Bounds _bounds;

	[SerializeField]
	private float _additionalAngle;

	[SerializeField]
	private Matrix4x4 _finalTransform;

	private void Reset() {

		var renderer = GetComponentInChildren<Renderer>( includeInactive: true );
		if ( renderer != null ) {

			_bounds = renderer.bounds;
		} else {

			var collider = GetComponentInChildren<Collider>( includeInactive: true );
			if ( collider != null ) {

				_bounds = collider.bounds;
			}
		}
	}

	public bool IsAffectingPoint( Vector3 point ) {

		var localPoint = _finalTransform.MultiplyPoint3x4( point );

		return _bounds.Contains( localPoint );
	}

	public void SetLocalBounds( Bounds warFogOccluderBounds ) {

		_bounds = warFogOccluderBounds;
	}

	public void SetAdditionalAngle( float additionalAngle ) {

		_additionalAngle = additionalAngle;

		_finalTransform = Matrix4x4.TRS( Vector3.zero, Quaternion.AngleAxis( _additionalAngle, Vector3.forward ), Vector3.one ) * transform.worldToLocalMatrix;
	}

	void OnDrawGizmos() {

		Gizmos.matrix = _finalTransform.inverse;
		Gizmos.DrawWireCube( _bounds.center, _bounds.size );
	}
}