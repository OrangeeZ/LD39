using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WarFogTracer : MonoBehaviour {

	public float RadiusScale { get; set; }

	[SerializeField]
	private float _radius = 5f;

	private WarFogSpaceMap _warFogSpaceMap;

	private void OnDrawGizmos() {

		if ( !Application.isPlaying ) {

			Trace( _warFogSpaceMap = _warFogSpaceMap ?? FindObjectOfType<WarFogSpaceMap>() );
		}
	}

	void Start() {

		RadiusScale = 1f;

		WarFogController.Tracers.Add( this );
	}

	void OnDestroy() {

		WarFogController.Tracers.Remove( this );
	}

	public void SetRadiusScale( float scale ) {

		RadiusScale = scale;

		_warFogSpaceMap.ClearVisible();
	}

	public void Trace( WarFogSpaceMap warFogSpaceMap ) {

		_warFogSpaceMap = warFogSpaceMap;
		warFogSpaceMap.Trace( transform.position, Mathf.RoundToInt( _radius * RadiusScale ) );
	}

}