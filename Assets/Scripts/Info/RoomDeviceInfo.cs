using UnityEngine;
using System.Collections;
using csv;

public class RoomDeviceInfo : ScriptableObject, ICsvConfigurable {

	public string Id;
	public string Effect;

	public bool CanWork;

	public bool CanBeActive;
	public float RechargeTime;

	public float EffectValue;

	public bool HasSupercharge;
	public float SuperchargeChance;
	public float SuperchargeTime;
	public float SuperchargeValue;
	public float SuperchargeHits;

	public bool CanBeBroken;
	public float Durability;

	public void Configure( Values values ) {

		Effect = values.Get<string>( "Effect" );

		CanWork = values.Get<bool>( "Work" );

		CanBeActive = values.Get<bool>( "Active" );
		RechargeTime = values.Get<float>( "RechargeTime" );

		EffectValue = values.Get<float>( "EffValue" );

		HasSupercharge = values.Get<bool>( "SuperCharge" );
		values.Get( "SuperChargeChance", out SuperchargeChance );

		values.Get( "SuperChargeTime", out SuperchargeTime );
		values.Get( "SuperChargeValue", out SuperchargeValue );

		values.Get( "SuperChargeHits", out SuperchargeHits );

		values.Get( "Broken", out CanBeBroken );

		values.Get( "Durability", out Durability );
	}

}