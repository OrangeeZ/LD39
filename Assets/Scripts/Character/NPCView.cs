using UnityEngine;
using System.Collections;
using System;
using Packages.EventSystem;

public class NPCView : AObject {

	public Transform itemSpawnPoint;
	public NPCInfo npc;
	public bool destroyOnPickup;

	public ItemView itemView { get; private set; }

	private void Start() {

		itemView = npc.itemInfo.DropItem( itemSpawnPoint ?? transform );
		itemView.giver = this;
	}

	public void OnPickedUp( Character character ) {

		var playerCharacterStatusInfo = character.Status.Info as PlayerCharacterStatusInfo;
		if ( playerCharacterStatusInfo != null ) {

			character.Status.ModifierCalculator.Add( ModifierType.BaseMoveSpeed, new OffsetValue( playerCharacterStatusInfo.SpeedBonusPerItem, OffsetValue.OffsetValueType.Constant ) );
		}

		EventSystem.RaiseEvent( new ItemView.PickedUp { ItemView = itemView } );

		if ( destroyOnPickup ) {

			Destroy( gameObject );
		}
	}

}