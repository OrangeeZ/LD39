using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class DeathWaveTrigger : WaveTriggerBase {

	public override void Initialize() {

		EventSystem.Events.SubscribeOfType<Character.Died>( OnEvent );
	}

	private void OnEvent( Character.Died diedEvent ) {
		
		NotifyTrigger();
		//Debug.Log( diedEvent.character );
	}
}
