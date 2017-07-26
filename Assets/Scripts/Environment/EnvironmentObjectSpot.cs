using Packages.EventSystem;
using UnityEngine;

public class EnvironmentObjectSpot : AObject {

	[SerializeField]
	private EnvironmentObjectSpotInfo _info;

	[SerializeField]
	protected State _state;

	[SerializeField]
	private OrientationType _orientationType;

	[SerializeField]
	private bool _isInvincible = false;

	private GameObject _viewInstance;

	public enum OrientationType {

		Universal,
		RightSide,
		LeftSide

	}

	public enum State {

		Default,
		Empty,
		Infected,
		Destroyed

	}

	public bool IsReserved { get; private set; }

	private void Start() {

		if ( _state == State.Default ) {

			_viewInstance = gameObject;
		}
	}

	private void OnValidate() {

#if UNITY_EDITOR
		if ( UnityEditor.PrefabUtility.GetPrefabType( this ) == UnityEditor.PrefabType.Prefab ) {

			return;
		}
#endif

		name = string.Format( "ObjectSpot [{0}]", _orientationType );
	}

	public virtual void Destroy( Character hittingCharacter ) {

		if ( !_isInvincible ) {

			SetState( State.Destroyed );
		}

		EventSystem.RaiseEvent( new XenoTriggerEvent { Source = this } );
	}

	public virtual void TryResetState() {

		if ( _state != State.Destroyed ) {

			Destroy( _viewInstance );

			_state = State.Empty;

			SetReserved( false );
		}
	}

	public void SetState( State state ) {

		_state = state;

		switch ( state ) {

			case State.Infected:
				SetInfectedState();
				break;

			case State.Destroyed:
				Destroy( _viewInstance );
				break;
		}
	}

	public State GetState() {

		return _state;
	}

	protected virtual void SetInfectedState() {

		_viewInstance = Instantiate( GetRandomView() );
		_viewInstance.transform.SetParent( transform, worldPositionStays: false );
		_viewInstance.transform.ResetLocalTransform();
	}

	private GameObject GetRandomView() {

		switch ( _orientationType ) {

			case OrientationType.RightSide:
				return _info.GetRandomRightSidePrefab();

			case OrientationType.LeftSide:
				return _info.GetRandomLeftSidePrefab();

			default:
				return _info.GetRandomPrefab();
		}
	}

	public void SetReserved( bool value ) {

		IsReserved = value;
	}

}