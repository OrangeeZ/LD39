using csv;
using Packages.EventSystem;
using UnityEngine;

[CreateAssetMenu( menuName = "Create/Weapons/Ranged" )]
public class RangedWeaponInfo : WeaponInfo {

	[SerializeField]
	private Projectile _projectilePrefab;

	[SerializeField]
	private float _projectileSpeed;

	public int ClipSize;
	public float ReloadDuration;

	[SerializeField]
	private int _projectilesPerShot;

	[SerializeField]
	private float _shotConeAngle;

	[SerializeField]
	private AudioClip[] _sounds;

	[SerializeField]
	private int _ammoLimit = 100;

	[SerializeField]
	private int _ammoAmount = 100;

	[SerializeField]
	private float _projectileLifetime;

	[SerializeField]
	private float _splashDamageRadius;

	[SerializeField]
	private RangedWeaponBehaviourInfo _weaponBehaviourInfo;

	[SerializeField]
	private BuffItemInfo _abilityOnPickup;

	public class RangedWeapon : Weapon<RangedWeaponInfo> {

		public struct Fire : IEventBase {

			public Character Character;
			public RangedWeapon Weapon;

		}

		protected RangedWeaponInfo _rangedWeaponInfo;
		public int AmmoInClip { get; private set; }

		public float BaseAttackSpeed {
			get { return Character.Status.ModifierCalculator.CalculateFinalValue( ModifierType.BaseAttackSpeed, _typedInfo.BaseAttackSpeed ); }
		}

		public int ClipSize { get; protected set; }
		public float ReloadDuration { get { return _rangedWeaponInfo.ReloadDuration * ReloadDurationScale; } }

		public bool IsUnlimited {
			get { return _rangedWeaponInfo._ammoLimit < 0; }
		}

		public bool IsReloading {
			get { return _behaviour == null ? true : _behaviour.IsReloading; }
		}

		public Vector3 AttackDirection { get; private set; }

		private bool IsAttackAvailable {
			get { return !_behaviour.IsReloading && ( IsUnlimited || AmmoInClip >= 0 ); }
		}

		public float ReloadDurationScale = 1;

		private RangedWeaponBehaviour _behaviour;

		public RangedWeapon( RangedWeaponInfo info ) : base( info ) {
			_rangedWeaponInfo = info;
			AmmoInClip = _rangedWeaponInfo._ammoAmount;
			ClipSize = info.ClipSize;
		}

		public override void SetCharacter( Character character ) {

			base.SetCharacter( character );

			_behaviour = _typedInfo._weaponBehaviourInfo.GetBehaviour();
			_behaviour.Initialize( Inventory, this );

			if ( _typedInfo._abilityOnPickup != null ) {

				var buffItem = _typedInfo._abilityOnPickup.GetItem();
				buffItem.SetCharacter( character );
				buffItem.Apply();
			}
		}

		public override void Attack( Character target, EnemyCharacterStatusInfo statusInfo ) {

			if ( target == null || !IsAttackAvailable ) {
				return;
			}

			if ( AttackAction != null ) {
				AttackAction();
			}

			for ( var i = 0; i < _typedInfo._projectilesPerShot; ++i ) {

				var projectile = GetProjectileInstance();
				var targetDirection = ( target.Pawn.position - Character.Pawn.position ).Set( y: 0 ).normalized;

				AttackDirection = targetDirection;

				var projectileDirection = GetOffsetDirection( targetDirection, i );

				var finalDamage = ModifierCalculator.CalculateFinalValue( ModifierType.BaseDamage,
					_typedInfo.BaseDamage );
				projectile.Launch( Character, projectileDirection, _typedInfo._projectileSpeed, finalDamage, _typedInfo.CanFriendlyFire, _typedInfo._splashDamageRadius );
				if ( _behaviour.TryShoot() ) {
					AmmoInClip -= ClipSize;
				}

				if ( _behaviour.IsReloading ) {
					break;
				}
			}

			var sound = _typedInfo._sounds.RandomElement();
			if ( sound != null ) {

				AudioSource.PlayClipAtPoint( sound, Character.Pawn.position, 0.5f );
			}
		}

		public override void Attack( Vector3 direction ) {
			if ( !IsAttackAvailable ) {
				return;
			}

			if ( AttackAction != null ) {
				AttackAction();
			}

			var shotsFired = 0;

			for ( var i = 0; i < _typedInfo._projectilesPerShot; ++i ) {

				var projectile = GetProjectileInstance();
				var projectileDirection = GetOffsetDirection( direction, i );
				var finalDamage = ModifierCalculator.CalculateFinalValue( ModifierType.BaseDamage, _typedInfo.BaseDamage );

				projectile.Launch( Character, projectileDirection,
					_typedInfo._projectileSpeed, finalDamage, _typedInfo.CanFriendlyFire,
					_typedInfo._splashDamageRadius );

				AttackDirection = direction;
                EventSystem.RaiseEvent( new Fire { Character = Character, Weapon = this } );

				if ( _behaviour.TryShoot() ) {

					AmmoInClip -= ClipSize;
				}

				if ( _behaviour.IsReloading ) {

					break;
				}
			}

			var sound = _typedInfo._sounds.RandomElement();
			if ( sound != null ) {
				AudioSource.PlayClipAtPoint( sound, Character.Pawn.position, 0.5f );
			}
		}

		public override bool CanAttack( Character target ) {
			return Vector3.Distance( target.Pawn.position, Character.Pawn.position )
			       <= _typedInfo.AttackRange;
		}

		private Vector3 GetOffsetDirection( Vector3 direction, int index ) {

			var totalOffsetCount = _typedInfo._projectilesPerShot;
			var coneAngle = _typedInfo._shotConeAngle;

			if ( totalOffsetCount == 1 ) {

				return direction;
			}

			var normalizedOffsetIndex = (float) index / totalOffsetCount;
			var rotator = Quaternion.AngleAxis( Mathf.Lerp( -coneAngle, coneAngle, normalizedOffsetIndex ), Vector3.up );

			return rotator * direction;
		}

		private Projectile GetProjectileInstance() {
			var result = Instantiate( _typedInfo._projectilePrefab );
			result.Lifetime = _typedInfo._projectileLifetime;
			return result;
		}

	}

	public override Item GetItem() {

		return new RangedWeapon( this );
	}

	public override void Configure( Values values ) {

		base.Configure( values );

		ReloadDuration = BaseAttackSpeed;
		_ammoLimit = values.Get( "AmmoLimit", -1 );
		_ammoAmount = values.Get( "AmmoAmount", -1 );
		_projectileSpeed = values.Get( "Projectile Speed", 0f );
		_projectilesPerShot = values.Get( "BulletsPerBurst", 1 );
		_projectileLifetime = values.Get( "ProjectileLifetime", 1f );
		_shotConeAngle = values.Get( "BurstAngle", 0 );
		_splashDamageRadius = values.Get( "SplashRadius", float.NaN );
		_abilityOnPickup = values.GetScriptableObject<BuffItemInfo>( "AbilityOnPickup" );
		ClipSize = values.Get( "Clip Size", _projectilesPerShot );
		_projectilePrefab = values.GetPrefabWithComponent<Projectile>( "Projectile", fixName: false );
	}

}