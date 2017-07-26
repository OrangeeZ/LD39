using System;
using UnityEngine;
using UniRx;
using System.Collections;
using Packages.EventSystem;
using Utility;

[CreateAssetMenu( menuName = "Create/States/Approach target" )]
public class ApproachTargetStateInfo : CharacterStateInfo {

	[Header( "Settings" )]
	[SerializeField]
	private float _minRange = 1.5f;

	[SerializeField]
	private float _maxRange = 4f;

	[SerializeField]
	private bool _autoActivate = true;

	[SerializeField]
	private bool _clearTargetOnReach = false;

	[Serializable]
	public class State : CharacterState<ApproachTargetStateInfo> {

		private TargetPosition destination;
		private bool _isFirstTimeNotice = true;
		private bool _targetIsCharacter = false;

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			stateController.character.InputSource.targets.Subscribe( SetDestination );
		}

		public override bool CanBeSet() {

			var distanceToDestination = destination.HasValue ? Vector3.Distance( character.Pawn.position, destination.Value ) : -1f;

			return destination.HasValue
			       && distanceToDestination > typedInfo._minRange
			       && distanceToDestination < typedInfo._maxRange;
		}

		public override IEnumerable GetEvaluationBlock() {

			if ( _isFirstTimeNotice && _targetIsCharacter ) {

				var enemyInfo = character.Status.Info as EnemyCharacterStatusInfo;
				var sound = enemyInfo.EnemySpottedSound.RandomElement();

				if ( sound != null ) {

					AudioSource.PlayClipAtPoint( sound, character.Pawn.position );
				}

				if ( 1f.Random() <= character.speakProbability ) {

					EventSystem.RaiseEvent( new Character.Speech {Character = character, messageId = enemyInfo.SpeakLineId} );
				}

				_isFirstTimeNotice = false;
			}

			var pawn = character.Pawn;

			do {

				yield return null;

				pawn.SetDestination( destination.Value );

				yield return null;

				//pawn.SetDestination( destination.Value );

			} while ( pawn.GetDistanceToDestination() > typedInfo._minRange && pawn.GetDistanceToDestination() < typedInfo._maxRange );

			//if ( pawn.GetDistanceToDestination() > typedInfo._maxRange ) {

			//	_didNoticeCharacter = false;
			//}

			if ( typedInfo._clearTargetOnReach ) {

				pawn.ClearDestination();
				destination = null;
			}
		}

		public void SetDestination( object target ) {

			if ( target is Vector3 ) {

				destination = (Vector3) target;
			} else if ( target is Character ) {
				_targetIsCharacter = true;
				var destinationTarget = ( target as Character );
				destination = destinationTarget.Pawn.transform;

			} else if ( target is ItemView ) {
				destination = ( target as ItemView ).transform;
			}

			if ( typedInfo._autoActivate ) {
				stateController.TrySetState( this );
			}
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}