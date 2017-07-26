using UnityEngine;
using System.Collections;

public class RoomDeviceListener : MonoBehaviour {

	public RoomDevice RoomDevice { get; private set; }

	void OnTriggerEnter( Collider other ) {

		RoomDevice = other.gameObject.GetComponent<RoomDevice>();
	}

}
