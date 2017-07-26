using UnityEngine;
using System.Collections;
using csv;

public enum ExternalEnemyType {

	Permanent,
	Temp,
	Event

}

public class ExternalEnemyInfo : ScriptableObject, ICsvConfigurable {

	public ExternalEnemyType Type;
	public string Target;
	public int Damage;

	public float AttackCooldown;
	public string ProjectileType;
	public int Hp;

	public int SpawnRange;
	public int SpeedPenalty;

	public string ActivatedEffect;
	public int TimeToActivateEffect;

	public void Configure( Values values ) {

		values.GetEnum( "Type", out Type );
		values.Get( "Target", out Target );
		values.Get( "Damage", out Damage );

		values.Get( "AttackCooldown", out AttackCooldown );
		values.Get( "Proj", out ProjectileType );
		values.Get( "HP", out Hp );

		values.Get( "SpawnRange", out SpawnRange );
		values.Get( "SpeedSlow", out SpeedPenalty );

		values.Get( "Effect", out ActivatedEffect );
		values.Get( "EffectTimerMax", out TimeToActivateEffect );

	}

}