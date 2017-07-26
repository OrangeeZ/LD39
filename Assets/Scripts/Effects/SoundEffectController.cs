using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class SoundEffectController : MonoBehaviour {

	[SerializeField]
	private AudioClip _doorOpen;

	[SerializeField]
	private AudioClip _doorClose;

	[SerializeField]
	private AudioClip _roomPowerDown;

	[SerializeField]
	private AudioClip _healbotHealSuccess;

	[SerializeField]
	private AudioClip _healbotHealFailed;

	// Use this for initialization
	private void Start() {

		EventSystem.Events.SubscribeOfType<Room.EveryoneDied>( OnEveryoneDieInRoom );

		EventSystem.Events.SubscribeOfType<HealbotObject.TriedHeal>( OnTryHeal );
	}

	private void OnTryHeal( HealbotObject.TriedHeal eventObject ) {

		AudioSource.PlayClipAtPoint( eventObject.DidSucceed ? _healbotHealSuccess : _healbotHealFailed, eventObject.Healbot.position );
	}

	private void OnEveryoneDieInRoom( Room.EveryoneDied eventObject ) {

		if ( eventObject.Room.GetRoomType() != Room.RoomType.Default ) {

			AudioSource.PlayClipAtPoint( _roomPowerDown, Camera.main.transform.position );
		}
	}
}