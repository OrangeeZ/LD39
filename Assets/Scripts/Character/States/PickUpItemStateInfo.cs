using UniRx;
using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/States/Pick up item" )]
public class PickUpItemStateInfo : CharacterStateInfo {

	public float duration = .1f;

	private class State : CharacterState {

		private ItemView target;

		public State( CharacterStateInfo info )
			: base( info ) {

		}

		public override void Initialize( CharacterStateController stateController ) {

			base.Initialize( stateController );

			stateController.character.InputSource.targets.OfType<object, ItemView>().Subscribe( SetTarget );
		}

		public override bool CanBeSet() {

			return target != null;
		}

		public override IEnumerable GetEvaluationBlock() {

			var info = this.info as PickUpItemStateInfo;
			var timer = new StepTimer( info.duration );

			while ( timer.ValueNormalized < 1f ) {

				timer.Step( deltaTime );

				yield return null;
			}

			target.NotifyPickUp( character );
			character.Inventory.AddItem( target.item );
			target = null;
		}

		private void SetTarget( ItemView target ) {

			this.target = target;
		}
	}

	public override CharacterState GetState() {

		return new State( this );
	}
}
