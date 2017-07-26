using UnityEngine;
using System.Collections;
using Packages.EventSystem;

public class ItemView : AObject {

	public class PickedUp : IEventBase {

		public ItemView ItemView;

	}

	public Item item;
	public NPCView giver;

	public float fadeInDuration = 1f;

	public AnimationCurve scaleCurve;
	public AnimationCurve positionCurve;

	private IEnumerator Start() {

		var timer = new AutoTimer( fadeInDuration );

		while ( timer.ValueNormalized < 1f ) {

			transform.localScale = Vector3.one * scaleCurve.Evaluate( timer.ValueNormalized );

			yield return null;
		}
	}

	public void NotifyPickUp( Character character ) {

		if ( giver != null ) {

			giver.OnPickedUp( character );
		}
		
		EventSystem.RaiseEvent( new PickedUp { ItemView = this } );

		Destroy( gameObject );
	}

	public void SetColor( Color baseColor ) {

		//var renderers = GetComponentsInChildren<Renderer>();
		//foreach ( var each in renderers ) {

		//	each.material.SetColor( "_Color", baseColor );
		//}
	}

}