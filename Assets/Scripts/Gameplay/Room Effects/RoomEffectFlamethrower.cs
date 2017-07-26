using UnityEngine;
using System.Collections;

public class RoomEffectFlamethrower : RoomEffect {

	public override void Activate() {

		base.Activate();

		Debug.Log( "FLAME!!!!" );
	}
}
