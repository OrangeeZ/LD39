using System.Collections.Generic;
using System.Linq;
using System.Monads;
using UnityEngine;
using System.Collections;

public class BasicInventory : IInventory {

	public int Gold {
		get { return _gold; }
		set { _gold = value.Clamped( 0, int.MaxValue ); }
	}

	private readonly List<Item> items = new List<Item>();
	private readonly Dictionary<BodySlotType, Item> bodySlotsInfo = new Dictionary<BodySlotType, Item>();
	private readonly Dictionary<ArmSlotType, Item> armSlotsInfo = new Dictionary<ArmSlotType, Item>();
	private readonly Character character;

	private int _gold;

	public BasicInventory( Character character ) {

		this.character = character;
	}

	public bool AddItem( Item item ) {

		items.Add( item );

		item.SetCharacter( character );

		return true;
	}

	public void RemoveItem( Item item ) {

		items.Remove( item );
	}

	public void RemoveItem<T>() where T : Item {

		items.Remove( items.FirstOrDefault( _ => _ is T ) );
	}

	public int GetItemCount<T>() where T : Item {

		return items.OfType<T>().Count();
	}

	public IEnumerable<Item> GetItems() {

		return items;
	}

	public bool SetBodySlotItem( BodySlotType bodySlotType, Item item ) {

		if ( bodySlotsInfo.ContainsKey( bodySlotType ) ) {

			AddItem( bodySlotsInfo[bodySlotType] );
		}

		bodySlotsInfo[bodySlotType] = item;
		RemoveItem( item );

		return true;
	}

	public Item GetBodySlotItem( BodySlotType bodySlotType ) {

		return bodySlotsInfo.With( bodySlotType );
	}

	public bool SetArmSlotItem( ArmSlotType armSlotType, Item item ) {

		armSlotsInfo[armSlotType] = item;

		return true;
	}

	public Item GetArmSlotItem( ArmSlotType armSlotType ) {

		return armSlotsInfo.With( armSlotType );
	}
}
