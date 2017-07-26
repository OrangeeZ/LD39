using UniRx;
using UnityEngine;
using System.Collections;

public class NullInputSource : IInputSource {

	public IObservable<Vector3> moveInput { get; private set; }

	public IReadOnlyReactiveProperty<object> targets { get; private set; }

	public NullInputSource() {
		
		moveInput = new Vector3ReactiveProperty();
		targets = new ReactiveProperty<object>();
	}
}
