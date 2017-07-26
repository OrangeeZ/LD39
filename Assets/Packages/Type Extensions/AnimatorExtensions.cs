using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class AnimatorExtensions {

	public static void SetBool( this IEnumerable<Animator> self, string name, bool value ) {

		foreach ( var each in self ) {

			if ( !each.isActiveAndEnabled ) {

				continue;
			}

			each.SetBool( name, value );
		}
	}

}