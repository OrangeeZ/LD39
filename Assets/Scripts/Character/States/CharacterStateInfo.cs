using System;
using UnityEngine;

public abstract class CharacterStateInfo : ScriptableObject {

	[Header( "Animation settings" )]
	public string animatorStateName;
	public string weaponAnimatorStateName;

	public abstract CharacterState GetState();

}
