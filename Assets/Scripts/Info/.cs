using UnityEngine;
using csv;

[CreateAssetMenu(menuName = "Info/")]
public class  : ScriptableObject, ICsvConfigurable
{
	[RemoteProperty]
	public string ;

	[RemoteProperty]
	public string Effect ;

	[RemoteProperty]
	public string Work;

	[RemoteProperty]
	public string Active;

	[RemoteProperty]
	public float RechargeTime;

	[RemoteProperty]
	public float EffValue;

	[RemoteProperty]
	public string SuperCharge;

	[RemoteProperty]
	public float SuperChargeChance;

	[RemoteProperty]
	public float SuperChargeTime;

	[RemoteProperty]
	public float SuperChargeValue;

	[RemoteProperty]
	public float SuperChargeHits;

	[RemoteProperty]
	public string Broken;

	[RemoteProperty]
	public float Durability;

	public void Configure(Values values)
	{
	}
}
