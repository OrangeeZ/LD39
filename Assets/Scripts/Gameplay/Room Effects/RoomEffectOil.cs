using UnityEngine;
using System.Collections;

public class RoomEffectOil : RoomEffect {

	public override void Activate() {

		base.Activate();

		Debug.Log( "OIL!!!!" );
	}
}
