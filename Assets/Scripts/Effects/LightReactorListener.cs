using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class LightReactorListener : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	private void Start() {

		EventSystem.Events.SubscribeOfType<Room.EveryoneDied>( OnEveryoneDieInRoom );
	}

	void Reset() {
		
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>(includeInactive: true);
	}

	private void OnEveryoneDieInRoom( Room.EveryoneDied eventObject ) {

		if ( eventObject.Room.GetRoomType() == Room.RoomType.Reactor ) {

			_spriteRenderer.enabled = false;
		}
	}

}