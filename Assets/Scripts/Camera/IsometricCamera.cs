using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class IsometricCamera : CameraBehaviour {

	public float deathOffset = 10;

	public float maxDistance = 5f;

	public float followTimeNormalized = 0.5f;

	private IEnumerable LerpToDistance() {

		var timer = new AutoTimer( 1f );

		var from = transform.localPosition;
		var to = transform.localPosition + Vector3.up * deathOffset;

		while ( timer.ValueNormalized < 1f ) {

			transform.localPosition = Vector3.Lerp( from, to, timer.ValueNormalized );

			yield return null;
		}
	}

	protected override void UpdateCamera() {

		var offset = transform.position - target.position;
		var clampedOffset = Vector3.ClampMagnitude( offset, maxDistance );

		transform.position += clampedOffset - offset;

		transform.position = Vector3.Lerp( transform.position, target.position, followTimeNormalized * Time.deltaTime );
		//transform.rotation = Quaternion.Lerp( transform.rotation, target.rotation, followTimeNormalized * Time.deltaTime );
	}

}