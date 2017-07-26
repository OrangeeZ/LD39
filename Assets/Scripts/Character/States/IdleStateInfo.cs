using UnityEngine;
using System.Collections;
using System.ComponentModel;

[CreateAssetMenu( menuName = "Create/States/Idle" )]
public class IdleStateInfo : CharacterStateInfo {

	public class State : CharacterState {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		//public override IEnumerable GetEvaluationBlock() {

		//	while ( true ) {

		//		yield return null;
		//	}
		//}
	}

	public override CharacterState GetState() {

		return new State( this );
	}
}
