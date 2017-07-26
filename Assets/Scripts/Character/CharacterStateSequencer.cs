using System.Linq;
using System.Collections;

public class CharacterStateSequencer : CharacterState {

	protected CharacterState[] states;

	public CharacterStateSequencer( CharacterStateInfo info ) : base( info ) {
	}

	public override IEnumerable GetEvaluationBlock() {

		stateController.SetScheduledStates( states.Where( _ => _.CanBeSet() ) );

		yield return null;
	}
}
