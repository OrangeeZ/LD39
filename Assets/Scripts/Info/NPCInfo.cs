using UnityEngine;
using System.Collections;

public class NPCInfo : ScriptableObject, ICsvConfigurable
{
	public ItemInfo itemInfo;
	public NPCView groundView;
	public float RechargeTimer;

	public float CharacterScaleBonus;

	public void Configure (csv.Values values)
	{
		var weapon = values.GetScriptableObject<ItemInfo>("ChangeWeapon");
		var ability = values.GetScriptableObject<ItemInfo>("Ability");

		itemInfo = weapon ?? ability;

		CharacterScaleBonus = values.Get( "CharacterScaleBonus", 0f );
		RechargeTimer = values.Get( "RechargeTimer", float.NaN );
	}
}

