using System;
using UnityEngine;
using System.Collections;
using csv;

[CreateAssetMenu( menuName = "Create/Items/Buff item info" )]
public class BuffItemInfo : ItemInfo, ICsvConfigurable {

	public ModifierType ModifierType;
	public OffsetValue ModifierValue;
	
	private class BuffItem : Item {

		private readonly BuffItemInfo _info;

		public BuffItem( BuffItemInfo info ) : base( info ) {

			_info = info;

		}

		public override void Apply() {

			Character.Status.ModifierCalculator.Add( _info.ModifierType, _info.ModifierValue );

			Character.Inventory.RemoveItem( this );
		}

	}

	public override Item GetItem() {

		return new BuffItem( this );
	}

	public void Configure( Values values ) {

		ModifierType = ModifierUtility.ParseModifierType( values.Get( "AffectedStat", string.Empty ) );
		ModifierValue = new OffsetValue( values.Get( "Amount", -1f ), ModifierUtility.ParseOffsetValueType( values.Get( "ActType", string.Empty ) ) );
		Name = values.Get( "Name", string.Empty );
	}
}