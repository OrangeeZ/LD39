using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class RoomEffectDrug : RoomEffect {
	
	public override void Activate() {

		base.Activate();

		TargetCharacter.StatModifier = EffectValue;

		Observable.Timer( new TimeSpan( 0, 0, 0, (int) Duration ) ).Subscribe( OnUpdate );
	}


	private void OnUpdate( long l ) {

		TargetCharacter.StatModifier = 1f;

		Debug.Log( "Drug ended" );
	}

}