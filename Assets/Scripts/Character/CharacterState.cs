using System.Collections.Generic;
using System.Linq;
using System.Collections;

public abstract class CharacterState {

	public readonly CharacterStateInfo info;

	protected Character character {
		get { return stateController.character; }
	}

	protected HashSet<CharacterState> possibleStates = new HashSet<CharacterState>();

	protected CharacterStateController stateController;

	protected float deltaTime;

	protected CharacterState( CharacterStateInfo info ) {

		this.info = info;
	}

	public void SetDeltaTime( float deltaTime ) {

		this.deltaTime = deltaTime;
	}

	public void SetTransitionStates( IEnumerable<CharacterState> states ) {

		foreach ( var each in states ) {

			possibleStates.Add( each );
		}
	}

	public CharacterState GetNextState() {

		return possibleStates.FirstOrDefault( which => which.CanBeSet() );
	}

	public virtual void Initialize( CharacterStateController stateController ) {

		this.stateController = stateController;
	}

	public virtual bool CanSwitchTo( CharacterState nextState ) {

		return possibleStates.Contains( nextState );
	}

	public virtual bool CanBeSet() {

		return true;
	}

	public virtual IEnumerable GetEvaluationBlock() {

		yield return null;
	}

	public void UpdateAnimator() {

		if ( character.Pawn != null ) {

			OnAnimationUpdate( character.Pawn );
		}
		//stateController.character.Pawn.animatedView.Do( OnAnimationUpdate );
	}

	protected virtual void OnAnimationUpdate( CharacterPawn pawn ) {

		if ( pawn._animationController != null ) {

			pawn._animationController.SetBool( info.animatorStateName, true );
		}

		if ( pawn._weaponAnimationController != null ) {

			pawn._weaponAnimationController.SetBool( info.weaponAnimatorStateName, true );
		}
	}

}

public class CharacterState<T> : CharacterState where T : CharacterStateInfo {

	protected readonly T typedInfo;

	public CharacterState( CharacterStateInfo info ) : base( info ) {

		typedInfo = info as T;
	}

}