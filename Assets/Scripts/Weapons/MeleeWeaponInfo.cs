using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu( menuName = "Create/Weapons/Melee" )]
public class MeleeWeaponInfo : WeaponInfo {

	[SerializeField]
	private float _attackAngle = 15f;

	[SerializeField]
	private float _attackRange = 2f;

	private class MeleeWeapon : Weapon<MeleeWeaponInfo> {

		//public SimpleSphereCollider sphereCollider;

		private float nextAttackTime;

		public MeleeWeapon( MeleeWeaponInfo info )
			: base( info ) {

		}

		public override void Attack( Character target, EnemyCharacterStatusInfo statusInfo ) {

			if ( Time.timeSinceLevelLoad < nextAttackTime ) {

				return;
			}

			if ( target == null ) {

				return;
			}

			nextAttackTime = Time.timeSinceLevelLoad + _typedInfo.BaseAttackSpeed;

			//if ( character.Status.GetHitChance() < 100.Random() ) {

			//	Debug.Log( "Miss!" );

			//	return;
			//}

			target.Damage( _typedInfo.BaseDamage );

			//if ( character.Status.GetCriticalHitChance( target.Status ) > 100.Random() ) {

			//	Debug.Log( "Critical hit!" );

			//	target.Damage( character.Status.GetMeleeAttack( info.damage ) * 5 / 3, ignoreArmor: true );
			//} else {

			//	target.Damage( character.Status.GetMeleeAttack( info.damage ) );
			//}

			//Debug.Log( character.Status.GetAttackDelay( info.attackDuration ) );
		}

		public override void Attack( Vector3 direction ) {

			var charactersToAttack = Helpers.GetCharactersInCone( Character.Pawn.position, direction, _typedInfo._attackRange, _typedInfo._attackAngle );
			foreach ( var each in charactersToAttack.ToArray() ) {

				if ( Character != each ) {

					each.Damage( _typedInfo.BaseDamage );
				}
			}
		}

		public override bool CanAttack( Character target ) {

			return ( target.Pawn.position - Character.Pawn.position ).sqrMagnitude <= _typedInfo.AttackRange.Pow( 2 );
		}

	}

	public override Item GetItem() {

		return new MeleeWeapon( this );
	}

}