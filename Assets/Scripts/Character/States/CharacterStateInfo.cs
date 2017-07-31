using System;
using UnityEngine;

public abstract class CharacterStateInfo : ScriptableObject {

	[Header( "Animation settings" )]
	public string animatorStateName;
	public bool IsAnimationExclusive = true;
	
	public string weaponAnimatorStateName;
	public bool IsWeaponAnimationExclusive = true;

	public abstract CharacterState GetState();

}
