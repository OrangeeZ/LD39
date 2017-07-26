
using System.Collections.Generic;

public enum BodySlotType {

	Body,
	Head,
	Legs
}

public enum ArmSlotType {

	Primary,
	Secondary
}

public interface IInventory {

	int Gold { get; set; }

	bool AddItem( Item item );

	void RemoveItem( Item item );

	void RemoveItem<T>() where T : Item;

	int GetItemCount<T>() where T: Item;

	IEnumerable<Item> GetItems();

	bool SetBodySlotItem( BodySlotType bodySlotType, Item item );

	Item GetBodySlotItem( BodySlotType bodySlotType );

	bool SetArmSlotItem( ArmSlotType armSlotType, Item item );

	Item GetArmSlotItem( ArmSlotType armSlotType );
}