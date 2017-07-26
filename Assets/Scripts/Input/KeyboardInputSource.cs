using UniRx;
using UnityEngine;
using System.Collections;

public class KeyboardInputSource {//}: IInputSource {

	public IObservable<Vector3> moveInput { get; private set; }

	public IObservable<Vector3> destinations { get; private set; }

	private Subject<Vector3> moveSubject = new Subject<Vector3>();

	public KeyboardInputSource() {

		Observable.EveryUpdate().Subscribe( Update );

		moveInput = moveSubject;

		destinations = new Subject<Vector3>();
	}

	private void Update( long ticks ) {

		moveSubject.OnNext( new Vector3( Input.GetAxis( "Horizontal" ), 0, Input.GetAxis( "Vertical" ) ) );
	}
}
