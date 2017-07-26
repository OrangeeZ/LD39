using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UniRx;
using UnityEngine;

public class SelectStateOnTargetChange : CharacterStateInfo {

	public CharacterStateInfo[] states;

	private class State : CharacterStateSequencer {

		public State( CharacterStateInfo info, CharacterState[] states ) : base( info ) {

			this.states = states;
		}

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			states = stateController.states.Where( where => where.info == info ).ToArray();

			//foreach ( var each in states ) {

			//	each.Initialize( stateController );
			//}

			stateController.character.InputSource.targets.Subscribe( OnTargetChange );
		}

		public override bool CanBeSet() {

			return true;
		}

		private void OnTargetChange( object target ) {

			stateController.TrySetState( this );
		}
	}

	public override CharacterState GetState() {

		var sequencerStates = states.Select( _ => _.GetState() ).ToArray();

		return new State( this, sequencerStates );
	}
}
