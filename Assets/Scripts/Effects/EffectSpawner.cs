using System;
using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityStandardAssets.Effects;

public class EffectSpawner : AObject {

	public GameObject CharacterDeathEffect;
	public GameObject BossDeathEffect;
	public GameObject PropsDeathEffect;
	public GameObject ShockGunEffect;

	private void Start() {

		EventSystem.Events.SubscribeOfType<Character.Died>( OnCharacterDie );
		EventSystem.Events.SubscribeOfType<RangedWeaponInfo.RangedWeapon.Fire>( OnWeaponFire );
		EventSystem.Events.SubscribeOfType<XenoTriggerEvent>( OnXenoTrigger );
	}

	private void OnXenoTrigger( XenoTriggerEvent eventObject ) {

		Instantiate( PropsDeathEffect, eventObject.Source.position );
	}

	private void OnWeaponFire( RangedWeaponInfo.RangedWeapon.Fire eventObject ) {

		Instantiate( ShockGunEffect, eventObject.Character.Pawn.position, Quaternion.FromToRotation( Vector3.right, eventObject.Weapon.AttackDirection ) );
	}

	private void OnCharacterDie( Character.Died diedEvent ) {

		Instantiate( CharacterDeathEffect, diedEvent.Character.Pawn.position, diedEvent.Character.Pawn.rotation );
	}

}