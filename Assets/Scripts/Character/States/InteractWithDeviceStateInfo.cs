using UnityEngine;
using System.Collections;
using UniRx;

[CreateAssetMenu( menuName = "Create/States/Interact With Device" )]
public class InteractWithDeviceStateInfo : CharacterStateInfo {

	public float duration = .5f;

	private class State : CharacterState<InteractWithDeviceStateInfo> {

		public State( CharacterStateInfo info )
			: base( info ) {
		}

		private RoomDevice _roomDevice;

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			Observable.EveryUpdate().Subscribe( CheckInput );
		}

		public override bool CanBeSet() {

			return _roomDevice != null && ( _roomDevice.IsInteractive() || _roomDevice.IsBroken() );
		}

		public override IEnumerable GetEvaluationBlock() {

			var timer = new AutoTimer( typedInfo.duration / character.StatModifier );

			_roomDevice.IsBeingRepared = true;

			while ( timer.ValueNormalized < 1 ) {

				yield return null;
			}

			if ( _roomDevice.IsBroken() ) {

				_roomDevice.SetFixed();
			} else {

				_roomDevice.Interact(character);
			}

			_roomDevice.IsBeingRepared = false;

			_roomDevice = null;
		}

		private void CheckInput( long ticks ) {

			if ( Input.GetButton( "Interact" ) ) {

				_roomDevice = character.Pawn.RoomDeviceListener.RoomDevice;

				stateController.TrySetState( this );
			}
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}