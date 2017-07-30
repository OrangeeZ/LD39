using UnityEngine;
using csv;

public class GlobalGameInfo : ScriptableObject, ICsvConfigurable
{
	[RemoteProperty]
	public float TimeAccelerationRate;

	[RemoteProperty]
	public float StartingTimeScale;

	public void Configure(Values values)
	{
	}
}
