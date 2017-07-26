using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using csv;

public class PlayerCharacterStatusInfo : CharacterStatusInfo {

	public ModifierType[] ModifierTypes;
	public OffsetValue[] OffsetValues;

	public RangedWeaponInfo BaseWeapon;

	public float SpeedBonusPerItem;

	public override CharacterStatus GetInstance() {

		var result = base.GetInstance();
		for ( var i = 0; i < ModifierTypes.Length; ++i ) {

			result.ModifierCalculator.Add( ModifierTypes[i], OffsetValues[i] );
		}

		return result;
	}

	public override void Configure( Values values ) {

		base.Configure( values );

		var fields = new Dictionary<string, ModifierType> {{"BaseRegeneration", ModifierType.BaseRegeneration}, {"BurningTimer", ModifierType.BurningTimerDuration}, {"DebuffTimer", ModifierType.DebuffTimerDuration}, {"WaterHPRestore", ModifierType.WaterHealthRestore}, {"SunHPrestore", ModifierType.SunHealthRestore}, {"ManureHPAdd", ModifierType.ManureHealthRestore}, {"DamageKoef", ModifierType.BaseDamage}, {"MaxAcorns", ModifierType.MaxAcorns}};

		MaxHealth = values.Get( "baseHP", MaxHealth );
		MoveSpeed = values.Get( "baseSpeed", MoveSpeed );

		ModifierTypes = new ModifierType[fields.Count];
		OffsetValues = new OffsetValue[fields.Count];

		var i = 0;
		foreach ( var each in fields ) {

			ModifierTypes[i] = fields[each.Key];
			OffsetValues[i] = new OffsetValue( values.Get( each.Key, 0f ), OffsetValue.OffsetValueType.Constant );

			i++;
		}

		BaseWeapon = values.GetScriptableObject<RangedWeaponInfo>( "BaseWeapon" );

		SpeedBonusPerItem = values.Get( "SpeedUpProgress", 0f );
	}

}