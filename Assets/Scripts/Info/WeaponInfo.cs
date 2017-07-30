using System;
using csv;

public class WeaponInfo : ItemInfo, ICsvConfigurable
{
    [RemoteProperty]
    public float BaseDamage;

    [RemoteProperty]
    public float BaseAttackSpeed;

    [RemoteProperty]
    public float AttackRange = 2f;

    public bool CanFriendlyFire = false;

    public ArmSlotType SlotType = ArmSlotType.Primary;

    public virtual void Configure(Values values)
    {
        SlotType = (ArmSlotType)Enum.Parse(typeof(ArmSlotType), values.Get("SlotType", string.Empty));
    }
}