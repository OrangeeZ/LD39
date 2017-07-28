using UnityEngine;
using csv;

public class GlobalGameInfo : ScriptableObject, ICsvConfigurable
{
	[RemoteProperty]
	public string MaxSpeed;

	[RemoteProperty]
	public float StartSpeed;

	[RemoteProperty]
	public float GlobalSpeedLow;

	[RemoteProperty]
	public float EnterpriseSpeedEnable;

	[RemoteProperty]
	public float EnterpriseSpeedDisable;

	[RemoteProperty]
	public float HeroSpeed;

	[RemoteProperty]
	public float Jump;

	[RemoteProperty]
	public float RepairCount;

	public void Configure(Values values)
	{
	}
}
