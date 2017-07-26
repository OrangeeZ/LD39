using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class HealbotObject : EnvironmentObjectSpot {

	public class TriedHeal :IEventBase {

		public HealbotObject Healbot;
		public bool DidSucceed;

	}

	[SerializeField]
	private float _cooldown = 5f;

	[SerializeField]
	private GameObject _activeState;

	[SerializeField]
	private GameObject _inactiveState;

	private float _cooldownTime;

	private void Start() {

		EventSystem.Events.SubscribeOfType<Room.EveryoneDied>( OnEveryoneDieInRoom );
	}

	private void OnEveryoneDieInRoom( Room.EveryoneDied everyoneDiedEvent ) {

		if ( everyoneDiedEvent.Room.GetRoomType() != Room.RoomType.MedicalBay ) {

			return;
		}

		enabled = false;

		_activeState.SetActive( false );
		_inactiveState.SetActive( true );
	}

	public override void Destroy( Character hittingCharacter ) {

		if ( Time.time < _cooldownTime || !enabled ) {

			EventSystem.RaiseEvent( new TriedHeal {Healbot = this, DidSucceed = false} );

			return;
		}

		hittingCharacter.Heal( 999 );

		EventSystem.RaiseEvent( new TriedHeal { Healbot = this, DidSucceed = true } );

		_cooldownTime = Time.time + _cooldown;
	}

	private void Update() {

		var isReady = Time.time > _cooldownTime;

		_activeState.SetActive( isReady );
		_inactiveState.SetActive( !isReady );
	}

}