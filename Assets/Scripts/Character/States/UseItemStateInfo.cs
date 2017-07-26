using UnityEngine;
using System.Collections;
using UniRx;

public class UseItemStateInfo : CharacterStateInfo {

	public float duration = .5f;

	private class State : CharacterState<UseItemStateInfo> {

		private Item item;

		public State( CharacterStateInfo info )
			: base( info ) {
		}

		public override bool CanBeSet() {
		
			return item != null;
		}

		public override IEnumerable GetEvaluationBlock() {

			var timer = new StepTimer( typedInfo.duration );

			while ( timer.ValueNormalized < 1 ) {

				timer.Step( deltaTime );

				yield return null;
			}

			item = null;
		}

		public void SetItem( Item item ) {

			this.item = item;
		}
	}

	public override CharacterState GetState() {

		return new State( this );
	}
}
