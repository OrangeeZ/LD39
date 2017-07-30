using UnityEngine;
using csv;

public class GlobalGameInfo : ScriptableObject, ICsvConfigurable
{
	[RemoteProperty]
	public float MaxPower;

	[RemoteProperty]
	public float StartingPower;

	[RemoteProperty]
	public float PowerDecreaseSpeed;
	
	public void Configure(Values values)
	{
	}
}
