using System.Linq;
using UnityEngine;
using System.Collections;

public class CharacterComplexAnimationController : MonoBehaviour {

	[SerializeField]
	private Animator[] animators;

	private string lastState = null;

	void Reset() {

		this.GetComponentsInChildren( out animators, includeInactive: true );
	}

	public void SetBool( string name, bool value ) {

		if ( name.IsNullOrEmpty() ) {

			return;
		}

		if ( !lastState.IsNullOrEmpty() ) {

			animators.SetBool( lastState, false );
		}

		animators.SetBool( name, value );
		
		lastState = name;
	}
}
