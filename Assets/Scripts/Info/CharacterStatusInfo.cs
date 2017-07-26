using UnityEngine;
using System.Collections;
using Expressions;
using UniRx;

[CreateAssetMenu( menuName = "Create/Status Info" )]
public class CharacterStatusInfo : ScriptableObject, ICsvConfigurable {

	[RemoteProperty("MaxHP")]
	public float MaxHealth;
	public float MoveSpeed;
	public float Damage;

	public float MaxAcorns;

	public AudioClip[] IdleSounds;
	public AudioClip[] DeathSounds;

	[SerializeField]
	private CharacterStatus status;

	public virtual CharacterStatus GetInstance() {

		return new CharacterStatus( this ) {

			Agility = {Value = status.Agility.Value},
			Strength = {Value = status.Strength.Value}
		};
	}

	public virtual void Configure( csv.Values values ) {

		MoveSpeed = values.Get( "Speed", MoveSpeed );
		Damage = values.Get( "DMG", Damage );
	}

}