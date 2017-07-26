using UnityEngine;
using System.Collections;
using Packages.EventSystem;

[CreateAssetMenu( menuName = "Create/States/Attack direction" )]
public class AttackDirectionStateInfo : CharacterStateInfo {

	private class State : CharacterState<AttackDirectionStateInfo> {

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public override bool CanBeSet() {

			var isButtonDown = Input.GetMouseButton	( 0 );

			return isButtonDown && ( Input.mousePosition - Camera.main.WorldToScreenPoint( character.Pawn.position ) ).magnitude > 0.1f;
		}

		public override IEnumerable GetEvaluationBlock() {

			while ( CanBeSet() ) {

				var weapon = character.Inventory.GetArmSlotItem( ArmSlotType.Primary ) as Weapon;

				var direction = ( Input.mousePosition - Camera.main.WorldToScreenPoint( character.Pawn.position ) );
				direction.z = direction.y;
				direction = direction.Set( y: 0 ).normalized;

				weapon.Attack( direction );
				
				yield return null;
			}
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}