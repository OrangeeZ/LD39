using System;
using UnityEngine;

public abstract class Item {

    public readonly ItemInfo info;

    protected IInventory Inventory;

    protected Character Character;

    protected ModifierCalculator ModifierCalculator;

    protected Item( ItemInfo info ) {

        this.info = info;
    }

    public virtual void SetCharacter( Character character ) {

        Character = character;
	    Inventory = character.Inventory;
        ModifierCalculator = character.Status.ModifierCalculator;
    }

    public virtual void Apply() {

        throw new NotImplementedException();
    }

}

public class ItemInfo : ScriptableObject {

    public string Name;

    public ItemView groundView;

    public Sprite inventoryIcon;

    public Color color;

    public virtual Item GetItem() {

        throw new NotImplementedException();
    }

    public ItemView DropItem( Transform transform ) {

        var item = GetItem();

        var view = Instantiate( groundView, transform.position, transform.rotation ) as ItemView;

        view.item = item;
        view.SetColor( color );

		return view;
    }

}