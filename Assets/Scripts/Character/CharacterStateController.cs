using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterStateController {

	public bool debug = false;
    public bool updateAnimation = false;

    [HideInInspector]
	public Character character;

	public IList<CharacterState> states = null;

	private CharacterState currentState { get; set; }

    private IEnumerator evaluationBlock = null;

	private Queue<CharacterState> scheduledStates = new Queue<CharacterState>();

	public void Initialize( Character character ) {

		this.character = character;

		foreach ( var each in states ) {

			each.Initialize( this );
		}
	}

	private void GetNextState() {

		var oldState = currentState;

		if ( scheduledStates.Any() ) {

			currentState = scheduledStates.Dequeue();
		} else if ( currentState != null ) {

			currentState = currentState.GetNextState();
		}

		if ( currentState == null ) {

			currentState = states.FirstOrDefault( that => that.CanBeSet() );
		}

		if ( debug && oldState != currentState ) {

			Debug.Log( string.Format( "{0}->{1}", oldState == null ? null : oldState.ToString(), currentState ) );
		}

		UpdateEvaluationBlock();
	}

	private void UpdateEvaluationBlock() {

		evaluationBlock = new PMonad().Add( currentState.GetEvaluationBlock() ).Add( GetNextState ).ToEnumerator();
	}

	public void Tick( float deltaTime ) {

		if ( evaluationBlock != null ) {

			currentState.SetDeltaTime( deltaTime );

			evaluationBlock.MoveNext();

		    if ( updateAnimation ) {

                currentState.UpdateAnimator();
		    }

		} else {

			GetNextState();
		}
	}

	public void TrySetState( CharacterState newState, bool allowEnterSameState = false ) {

		if ( newState != currentState || !allowEnterSameState ) {

			if ( ( currentState != null && !currentState.CanSwitchTo( newState ) ) || !newState.CanBeSet() ) {

				return;
			}
		}

		if ( debug ) {

			Debug.Log( string.Format( "{0}->{1}", currentState == null ? null : currentState.ToString(), newState ) );
		}
		
		currentState = newState;

		UpdateEvaluationBlock();
	}

	public void ForceSetState( CharacterState newState ) {

		if ( debug ) {

			Debug.Log( string.Format( "{0}->{1}", currentState == null ? null : currentState.ToString(), newState ) );
		}

		currentState = newState;

		UpdateEvaluationBlock();
	}

	public void TrySetState( CharacterStateInfo newStateInfo, bool allowEnterSameState = false ) {

		TrySetState( GetStateByInfo( newStateInfo ), allowEnterSameState );
	}

	public void SetScheduledStates( IEnumerable<CharacterState> states ) {

		scheduledStates.Clear();

		foreach ( var each in states ) {

			scheduledStates.Enqueue( each );
		}
	}

	public CharacterState GetStateByInfo( CharacterStateInfo info ) {

		return states.FirstOrDefault( where => where.info == info );
	}

	public T GetState<T>() where T : CharacterState {

		return states.OfType<T>().FirstOrDefault();
	}

	//public void SetEvaluationBlock( IEnumerator evaluationBlock ) {

	//	this.evaluationBlock = evaluationBlock;
	//}
}
