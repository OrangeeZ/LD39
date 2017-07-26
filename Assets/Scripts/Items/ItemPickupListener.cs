using UnityEngine;
using System.Collections;
using Packages.EventSystem;

public class ItemPickupListener : MonoBehaviour {

	[SerializeField]
	private CharacterPawnBase _pawn;

	private void Reset() {

		this.GetComponent( out _pawn );
	}

	private void OnTriggerEnter( Collider other ) {

		var itemView = other.GetComponent<ItemView>();
		if ( itemView == null ) {

			var npcView = other.GetComponent<NPCView>();
			if ( npcView != null ) {
				itemView = npcView.itemView;
			}
		}

		if ( itemView != null ) {
			
			_pawn.character.Inventory.AddItem( itemView.item );

			itemView.item.SetCharacter( _pawn.character );

			itemView.item.Apply();

			itemView.NotifyPickUp( _pawn.character );
		}
	}

}