using System;
using UniRx;
using UnityEngine;
using System.Collections;

public class SphereSensor : MonoBehaviour, IObservable<CharacterPawnBase> {

	[SerializeField]
	private SphereCollider _collider;

    private readonly Subject<CharacterPawnBase> _pawnSubject = new Subject<CharacterPawnBase>();

	void OnEnable() {

		if ( _collider != null ) {
			
			_collider.enabled = true;
		}
	}

	void OnDisable() {

		if ( _collider != null ) {

			_collider.enabled = false;
		}
	}

	public void SetRadius( float radius ) {

		_collider.radius = radius;
	}

    public IDisposable Subscribe( IObserver<CharacterPawnBase> observer ) {

        return _pawnSubject.Subscribe( observer );
    }

    private void OnTriggerEnter( Collider other ) {

        var otherPawn = other.transform.root.gameObject.GetComponent<CharacterPawnBase>();

        if ( otherPawn != null ) {

            _pawnSubject.OnNext( otherPawn );
        }
    }

}