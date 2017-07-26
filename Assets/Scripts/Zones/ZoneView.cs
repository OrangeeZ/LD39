using System;
using UnityEngine;
using System.Collections;

public class ZoneView : MonoBehaviour {

	public Action<Character> CharacterEntered;
	public Action<Character> CharacterExited;

	[SerializeField]
	private Renderer _renderer;

	public virtual void UpdateIntensity( float intensity ) {

		var color = _renderer.material.GetColor( "_TintColor" );
		color.a = intensity;
		_renderer.material.SetColor( "_TintColor", color );
	}

	private void OnTriggerEnter( Collider other ) {

		var otherPawn = other.GetComponent<CharacterPawn>();
		if ( otherPawn != null && CharacterEntered != null ) {

			CharacterEntered( otherPawn.character );
		}
	}

	private void OnTriggerExit( Collider other ) {

		var otherPawn = other.GetComponent<CharacterPawn>();
		if ( otherPawn != null && CharacterExited != null ) {

			CharacterExited( otherPawn.character );
		}
	}

}