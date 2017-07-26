using System;
using UniRx;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIInputPanel : MonoBehaviour, IObservable<Vector2>, IPointerClickHandler {

	public static UIInputPanel instance { get; private set; }

	private Subject<Vector2> subject = new Subject<Vector2>();

	void Awake() {

		instance = this;
	}

	public IDisposable Subscribe( IObserver<Vector2> observer ) {

		return subject.Subscribe( observer );
	}

	public void OnPointerClick( PointerEventData eventData ) {

		subject.OnNext( eventData.position );
	}
}
