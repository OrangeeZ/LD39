using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TerminalMarker : MonoBehaviour {

	public GameObject WorkingState;
	public GameObject RepairState;
	public GameObject BrokenState;

	public Text Timer;

	public RoomDevice Target;

	private void Start() {

		GetComponent<UIElementWorldAnchor>().Target = Target.transform;
	}

	private void Update() {

		if ( Target.IsBroken() ) {

			SetBrokenState();
		} else if ( Target.IsBeingRepared ) {

			SetRepairState( 2 );
		} else {

			SetWorkingState();
		}
	}

	private void SetBrokenState() {

		BrokenState.SetActive( true );
		WorkingState.SetActive( false );
		RepairState.SetActive( false );
	}

	public void SetWorkingState() {

		RepairState.SetActive( false );
		BrokenState.SetActive( false );
		WorkingState.SetActive( true );
	}

	public void SetRepairState( int durationSeconds ) {

		SetTime( durationSeconds );

		BrokenState.SetActive( false );
		WorkingState.SetActive( false );
		RepairState.SetActive( true );
	}

	public void SetTime( int seconds ) {

		Timer.text = string.Format( "0{0}:0{1}", 0, seconds );
	}

}