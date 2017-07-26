using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIElementWorldAnchor : MonoBehaviour {

	public Transform Target;
	
	private void Update() {

		if ( Target != null ) {

			var screenPosition = Camera.main.WorldToScreenPoint( Target.position );
			transform.position = screenPosition;
		}
	}

}