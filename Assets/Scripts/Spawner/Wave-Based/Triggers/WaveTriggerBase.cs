using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class WaveTriggerBase : ScriptableObject, IObservable<object> {

	//public event System.Action OnTrigger;

	private readonly Subject<object> _subject = new Subject<object>(); 

	public virtual void Initialize() {
		

	}

	protected void NotifyTrigger() {

		_subject.OnNext( null );

		//if ( OnTrigger != null ) {

		//	OnTrigger();
		//}
	}

	public IDisposable Subscribe( IObserver<object> observer ) {

		return _subject.Subscribe( observer );
	}

}