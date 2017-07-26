using System;
using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	protected CharacterPawnBase target;

	public void SetTarget( CharacterPawnBase target ) {

		this.target = target;
	}

	protected virtual void UpdateCamera() {


	}

	void LateUpdate() {

		if ( target != null ) {

			UpdateCamera();
		} else {

			throw new Exception( "Camera target is not set" );
		}
	}
}
