using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class TestRoomListener : MonoBehaviour {

	private void Start() {

		EventSystem.Events.SubscribeOfType<Room.EveryoneDied>( OnEveryoneDieInRoom );
	}

	private void OnEveryoneDieInRoom( Room.EveryoneDied everyoneDiedEvent ) {

		Debug.LogFormat( "Everyone died in {0}", everyoneDiedEvent.Room );
	}

}