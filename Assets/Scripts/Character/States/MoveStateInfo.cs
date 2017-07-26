using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/States/Move" )]
public class MoveStateInfo : CharacterStateInfo {

	public class State : CharacterState<MoveStateInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			return GetMoveDirection().magnitude > 0 && character.Pawn.IsGrounded() && !Input.GetButton( "Jump" );
		}

		public override IEnumerable GetEvaluationBlock() {

			while ( CanBeSet() ) {

				character.Pawn.SetSpeed( character.Status.MoveSpeed.Value * character.StatModifier );
				character.Pawn.MoveHorizontal( GetMoveDirection() );

				yield return null;
			}
		}

		private Vector3 GetMoveDirection() {

			return new Vector3( Input.GetAxis( "Horizontal" ), 0, 0 ).ClampMagnitude( 1f ); //GameScreen.instance.moveJoystick.GetValue();
			return new Vector3( Input.GetAxis( "Horizontal" ), 0, Input.GetAxis( "Vertical" ) ).ClampMagnitude( 1f ); //GameScreen.instance.moveJoystick.GetValue();
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}