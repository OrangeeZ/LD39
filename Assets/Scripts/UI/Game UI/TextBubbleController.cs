using System;
using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class TextBubbleController : MonoBehaviour {

	[SerializeField]
	protected TextBubblePopup _textBubble;

	[SerializeField]
	protected MessagesInfo _messages;

	public bool IsItemsInfo = false;

	private void Start() {
		if ( !IsItemsInfo ) {

			EventSystem.Events.SubscribeOfType<Character.Speech>( OnCharacterSpeak );
		} else {

			EventSystem.Events.SubscribeOfType<ItemView.PickedUp>( OnItemPickUp );
		}
	}

	private void OnItemPickUp( ItemView.PickedUp pickedUp ) {

		if ( Camera.main == null ) {

			return;
		}

		if ( !string.IsNullOrEmpty( pickedUp.ItemView.item.info.Name ) ) {

			var text = "+ " + _messages.GetText( pickedUp.ItemView.item.info.Name );

			var instance = Instantiate( _textBubble );
			instance.transform.SetParent( transform );
			instance.gameObject.SetActive( true );
			instance.Initialize( pickedUp.ItemView.transform, text );
		}
	}

	private void OnCharacterSpeak( Character.Speech speech ) {

		if ( Camera.main == null || speech.Character.Pawn == null ) {

			return;
		}

		string text;

		if ( !string.IsNullOrEmpty( speech.messageId ) ) {
			text = _messages.GetText( speech.messageId );
		} else {
			text = _messages.GetRandom();
		}

		var instance = Instantiate( _textBubble );
		instance.transform.SetParent( transform );
		instance.gameObject.SetActive( true );
		instance.Initialize( speech.Character.Pawn.transform, text );
	}

}