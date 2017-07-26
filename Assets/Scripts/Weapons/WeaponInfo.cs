using csv;

public class WeaponInfo : ItemInfo, ICsvConfigurable {

	public float BaseDamage;

	public float BaseAttackSpeed;

	public float AttackRange = 2f;

	public bool CanFriendlyFire = false;

	public ArmSlotType SlotType = ArmSlotType.Primary;

	public virtual void Configure( Values values ) {

		BaseDamage = values.Get( "DMG", 0f );
		BaseAttackSpeed = values.Get( "Base attack speed", 1f );

		var attackRangeValue = values.Get( "AtkRange", "0f" );
		if ( attackRangeValue == "max" )
        {
			AttackRange = int.MaxValue;
		} else {

			float.TryParse( attackRangeValue, out AttackRange );
		}

		CanFriendlyFire = values.Get( "FriendlyFire", "no" ) == "yes";
	}

}
