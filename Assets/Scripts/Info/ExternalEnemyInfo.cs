using UnityEngine;
using csv;

public class ExternalEnemyInfo : ScriptableObject, ICsvConfigurable
{
	public System.String ID;

	public System.String Type;

	public System.String Target;

	public System.Single Damage;

	public System.Single AttackCooldown;

	public System.String Proj;

	public System.Single HP;

	public System.Single SpawnRange;

	public System.Single SpeedSlow;

	public System.String Effect;

	public System.Single EffectTimerMax;

	public void Configure(Values values)
	{
	}
}
