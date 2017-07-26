using System;
using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class TurnEnemy : ExternalEnemy {

	private IDisposable _eventSubscription;
	protected bool _isTurnActive = false;

	public override void Initialize() {

		_eventSubscription = EventSystem.Events.SubscribeOfType<RoomDevice.Activated>( OnRoomDeviceActivate );

		StartCoroutine( Loop() );
	}

	private IEnumerator Loop() {

		var timer = new AutoTimer( EnemyInfo.TimeToActivateEffect );
		while ( timer.ValueNormalized < 1 ) {

			Debug.Log( ( EnemyInfo.TimeToActivateEffect - timer.Value ) + " until " + EnemyInfo.ActivatedEffect );

			if ( _isTurnActive ) {

				OnFinish();

				yield break;
			}

			yield return null;
		}

		OnFinish();

		AttackTarget.Damage( EnemyInfo.SpeedPenalty );
	}

	private void OnFinish() {

		_eventSubscription.Dispose();

		Controller.RemoveEnemy( this );
	}

	private void OnRoomDeviceActivate( RoomDevice.Activated activatedEvent ) {

		if ( string.Equals( activatedEvent.Device.RoomDeviceInfo.Effect, EnemyInfo.ActivatedEffect, StringComparison.CurrentCultureIgnoreCase ) ) {

			_isTurnActive = true;
		}
	}

}