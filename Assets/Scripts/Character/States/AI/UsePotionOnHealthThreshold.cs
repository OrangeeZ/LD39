using System.Linq;
using UniRx;
using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/States/Use potion on health threshold" )]
public class UsePotionOnHealthThreshold : CharacterStateInfo {

	public float duration = .5f;

	[Range( 0, 1 )]
	public float threshold = .5f;

	public ItemInfo potionItemInfo;

	private class State : CharacterState<UsePotionOnHealthThreshold> {

		public State( CharacterStateInfo info )
			: base( info ) {
		}

		public override void Initialize( CharacterStateController stateController ) {
			base.Initialize( stateController );

			stateController.character.Health
				.Where( _ => _ / (float)stateController.character.Status.MaxHealth.Value <= typedInfo.threshold )
				.Subscribe( OnHealthBelowThreshold );
		}

		public override bool CanBeSet() {

			var character = stateController.character;

			var hasPotion = character.Inventory.GetItems().FirstOrDefault( where => where.info == typedInfo.potionItemInfo ) != null;

			return hasPotion && character.Health.Value / (float)character.Status.MaxHealth.Value <= typedInfo.threshold;
		}

		public override IEnumerable GetEvaluationBlock() {

			var character = stateController.character;
			var potion = character.Inventory.GetItems().FirstOrDefault( where => where.info == typedInfo.potionItemInfo );

			potion.Apply();

			var timer = new AutoTimer( typedInfo.duration );
			while ( timer.ValueNormalized < 1f ) {

				yield return null;
			}
		}

		private void OnHealthBelowThreshold( float health ) {

			stateController.TrySetState( this );
		}
	}

	public override CharacterState GetState() {

		return new State( this );
	}
}
