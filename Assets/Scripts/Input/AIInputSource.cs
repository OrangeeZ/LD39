using System;
using UniRx;
using UnityEngine;
using System.Collections;

public class AIInputSource : IInputSource {

	private Character targetCharacter;

	public IObservable<Vector3> moveInput { get; private set; }

	public IReadOnlyReactiveProperty<object> targets { get; private set; }

	private readonly ReactiveProperty<object> targetsSubject = new ReactiveProperty<object>();

	public AIInputSource() {

		this.targets = targetsSubject;
	}

	public void Initialize( Character targetCharacter ) {

		this.targetCharacter = targetCharacter;

		targetCharacter.Pawn.GetSphereSensor()
			.Where( _ => _.character != targetCharacter )
			.Subscribe( OnSawEnemy );
	}

	private void OnSawEnemy( CharacterPawnBase characterPawn ) {

		targetsSubject.SetValueAndForceNotify( characterPawn.character );
	}
}
