using UnityEngine;
using System.Collections;
using UniRx;

public class TimerWaveTrigger : WaveTriggerBase {

	public float Duration;
	private bool _didFire;

	public override void Initialize() {

		//new PMonad().Add( Tick() ).Execute();
		new PMonad().Add( Tick() ).Add( NotifyTrigger ).Execute();
	}

	private IEnumerable Tick() {

		yield return null;

		var timeCurrent = 0f;

		while ( true ) {

			timeCurrent += Time.deltaTime;

			if ( timeCurrent >= Duration ) {

				yield break;
			}

			yield return null;
		}
	}

}