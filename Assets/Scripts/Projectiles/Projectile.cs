using Packages.EventSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : AObject {

	public float Lifetime;

	public float Damage { get; protected set; }

	public float LifeFraction {
		get { return _timer.ValueNormalized; }
	}

	public float weight = 1f;

	public bool DelayedDestroy = false;

	public bool NeedsSplashEffect = false;

	private AutoTimer _timer;

	protected Vector3 Direction;

	public Character Owner { get; protected set; }

	protected float Speed;
	protected bool CanFriendlyFire;
	private float _splashRange;

	private bool _isDestroyed = false;
	private int _frameCountStart;

	private void Awake() {

		enabled = false;
	}

	protected virtual void Update() {

		if ( _timer.ValueNormalized >= 1f ) {

			OnLifetimeExpire();
		}

		position += Direction * Speed * Time.deltaTime;

		if ( _isDestroyed && Time.frameCount - _frameCountStart >= 4 ) {

			Destroy( gameObject );
		}
	}

	public void Launch( Character owner, Vector3 direction, float speed, float damage, bool canFriendlyFire, float splashRange ) {

		this.Owner = owner;
		this.Speed = speed;
		this.Direction = direction;
		this.Damage = damage;
		this.CanFriendlyFire = canFriendlyFire;

		_splashRange = splashRange;
		_frameCountStart = Time.frameCount;

		transform.position = this.Owner.Pawn.GetWeaponPosition();
		transform.rotation = this.Owner.Pawn.rotation;

		_timer = new AutoTimer( Lifetime );

		var colorer = GetComponent<ProjectileColorer>();
		if ( colorer != null ) {
			colorer.Apply( Owner.IsEnemy() );
		}

		enabled = true;
	}

	public virtual void OnHit() {

		Release();
	}

	public virtual void OnContact( Collider other ) {

	}

	public virtual void OnLifetimeExpire() {

		Release();
	}

	protected virtual void Release() {

		if ( !_splashRange.IsNan() && _splashRange > 0f ) {

			Helpers.DoSplashDamage( transform.position, _splashRange, Damage, teamToSkip: CanFriendlyFire ? -1 : Owner.TeamId );

			if ( NeedsSplashEffect ) {

				EventSystem.RaiseEvent( new Helpers.SplashDamage {position = position, radius = _splashRange * 0.5f} );
			}
		}

		_isDestroyed = true;

		if ( !DelayedDestroy ) {

			Destroy( gameObject );
		}
	}

	private void OnTriggerEnter( Collider other ) {

		if ( _isDestroyed ) {

			return;
		}

		var otherPawn = other.GetComponent<CharacterPawnBase>();

		if ( otherPawn != null && otherPawn != Owner.Pawn && otherPawn.character != null ) {

			var canAttackTarget = !CanFriendlyFire || otherPawn.character.TeamId != Owner.TeamId;

			if ( canAttackTarget ) {

				otherPawn.character.Damage( Damage );

				OnContact( other );
				OnHit();
			}

			return;
		}

		OnContact( other );

		//if ( other.transform.parent != null ) 
			{

			var environmentObject = other.GetComponent<EnvironmentObjectSpot>();
			if ( environmentObject != null ) {

				environmentObject.Destroy( Owner );
			}
		}

		if ( other.tag == "Environment" ) {

			OnHit();
		}
	}

}