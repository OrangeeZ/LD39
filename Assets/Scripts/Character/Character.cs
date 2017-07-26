using System;
using System.Collections.Generic;
using Packages.EventSystem;
using UniRx;
using UnityEngine;

public class Character {

	public struct Died : IEventBase {

		public Character Character;

	}

	public struct RecievedDamage : IEventBase {

		public Character Character;
		public float Damage;

	}

	public struct Speech : IEventBase {

		public Character Character;
		public string messageId;

	}

	public static List<Character> Instances = new List<Character>();

	public readonly FloatReactiveProperty Health;

	public readonly IInputSource InputSource;

	public readonly IInventory Inventory;

	public readonly CharacterPawn Pawn;

	public readonly CharacterStateController StateController;
	public readonly CharacterStateController WeaponStateController;

	public readonly int TeamId;
	public readonly CharacterInfo Info;

	public bool IsPlayerCharacter = false;

	public readonly CharacterStatus Status;

	public ItemInfo[] ItemsToDrop;
	public float dropProbability = 0.15f;
	public float speakProbability = 0.15f;

	private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

	public float StatModifier = 1f;

	public Character( CharacterPawn pawn, IInputSource inputSource, CharacterStatus status, CharacterStateController stateController, CharacterStateController weaponStateController, int teamId, CharacterInfo info ) {

		Status = status;
		Health = new FloatReactiveProperty( Status.MaxHealth.Value );
		Pawn = pawn;
		InputSource = inputSource;
		StateController = stateController;
		WeaponStateController = weaponStateController;
		TeamId = teamId;
		Info = info;
		Inventory = new BasicInventory( this );

		pawn.SetCharacter( this );

		StateController.Initialize( this );
		WeaponStateController.Initialize( this );

		var inputSourceDisposable = inputSource as IDisposable;
		if ( inputSourceDisposable != null ) {

			_compositeDisposable.Add( inputSourceDisposable );
		}

		Observable.EveryUpdate().Subscribe( OnUpdate ).AddTo( _compositeDisposable );
		status.MoveSpeed.Subscribe( UpdatePawnSpeed ).AddTo( _compositeDisposable );
		Health.Subscribe( OnHealthChange ); //.AddTo( _compositeDisposable );

		Instances.Add( this );

		Status.ModifierCalculator.Changed += OnModifiersChange;
	}

	private void OnHealthChange( float health ) {

		if ( health <= 0 ) {

			EventSystem.RaiseEvent( new Died {Character = this} );

			if ( 1f.Random() <= speakProbability ) {

				EventSystem.RaiseEvent( new Speech { Character = this } );
			}

			Instances.Remove( this );

			//_compositeDisposable.Dispose();

			Status.ModifierCalculator.Changed -= OnModifiersChange;
		}
	}

	public void Heal( float amount ) {

		if ( Health.Value == Status.MaxHealth.Value ) {

			return;
		}

		var healAmount = Mathf.Min( Status.MaxHealth.Value - Health.Value, amount );

		if ( healAmount > 0 ) {
			
			Health.Value += healAmount;
		}
	}

	public void Damage( float amount ) {

		if ( Health.Value <= 0 ) {

			return;
		}

		Health.Value -= amount;
		
		EventSystem.RaiseEvent( new RecievedDamage {Character = this, Damage = amount} );
	}

	public bool IsEnemy() {
		return TeamId != 0;
	}

	public void Dispose() {

		if ( InputSource is IDisposable ) {

			(InputSource as IDisposable).Dispose();
		}

		_compositeDisposable.Dispose();
		Health.Dispose();
	}

	private void OnUpdate( long ticks ) {

		StateController.Tick( Time.deltaTime );
		WeaponStateController.Tick( Time.deltaTime );
	}

	private void UpdatePawnSpeed( float speed ) {

		Pawn.SetSpeed( speed );
	}

	private void OnModifiersChange() {

	}

}