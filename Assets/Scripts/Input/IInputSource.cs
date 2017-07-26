using UniRx;
using UnityEngine;
using System.Collections;

public interface IInputSource {

	IObservable<Vector3> moveInput { get; }

	IReadOnlyReactiveProperty<object> targets { get; }
}
