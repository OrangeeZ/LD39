using UnityEngine;
using csv;

public class ExternalEnemyInfo : ScriptableObject, ICsvConfigurable
{
	[RemoteProperty]
	public string ID;

	[RemoteProperty]
	public string Type;

	[RemoteProperty]
	public string Target;

	[RemoteProperty]
	public float Damage;

	[RemoteProperty]
	public float AttackCooldown;

	[RemoteProperty]
	public string Proj;

	[RemoteProperty]
	public float HP;

	[RemoteProperty]
	public float SpawnRange;

	[RemoteProperty]
	public float SpeedSlow;

	[RemoteProperty]
	public string Effect;

	[RemoteProperty]
	public float EffectTimerMax;

	public void Configure(Values values)
	{
	}
}
