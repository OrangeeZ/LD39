using System;
using UniRx;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Channels;

[CreateAssetMenu( menuName = "Create/States/Attack target" )]
public class AttackStateInfo : CharacterStateInfo {

	[SerializeField]
	private bool _attackOnce = false;

	[Serializable]
	public class State : CharacterState<AttackStateInfo> {

		private Character target;

		public State( CharacterStateInfo info ) : base( info ) {
		}

		public void SetTarget( Character targetCharacter ) {

			if ( targetCharacter != character ) {

				target = targetCharacter;
			}
		}

		public override bool CanBeSet() {

			var weapon = GetCurrentWeapon();

			return target != null && weapon != null && weapon.CanAttack( target );
		}

		public override IEnumerable GetEvaluationBlock() {

			var weapon = GetCurrentWeapon();

			while ( CanBeSet() ) {

				weapon.Attack( target, character.Status.Info as EnemyCharacterStatusInfo );

				character.Pawn.UpdateSpriteAnimationDirection( weapon.AttackDirection );

				yield return null;

				if ( typedInfo._attackOnce ) {

					break;
				}
			}
		}

		private RangedWeaponInfo.RangedWeapon GetCurrentWeapon() {

			return character.Inventory.GetArmSlotItem( ArmSlotType.Primary ) as RangedWeaponInfo.RangedWeapon;
		}

	}

	public override CharacterState GetState() {

		return new State( this );
	}

}