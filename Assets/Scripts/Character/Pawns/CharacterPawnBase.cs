using UniRx;
using UnityEngine;
using System.Collections;

public class CharacterPawnBase : AObject {

	protected float speed;
    
	[SerializeField]
	private SphereSensor sensor;
    
	[SerializeField]
	public CharacterComplexAnimationController _animationController;
	public CharacterComplexAnimationController _weaponAnimationController;

	public CharacterComplexAnimationController animatedView { get; private set; }

	public Character character { get; private set; }

	protected virtual void Start() {

	    animatedView = _animationController;
	}

	public SphereSensor GetSphereSensor() {

		return sensor;
	}

	public void SetCharacter( Character character ) {

		this.character = character;
	}

	public virtual void SetSpeed( float newSpeed ) {

		speed = newSpeed;
	}

	public virtual void MoveHorizontal( Vector3 direction ) {

		transform.position += speed * direction * Time.deltaTime;
	}

	public virtual void SetDestination( Vector3 destination ) {

		//navMeshAgent.destination = destination;
	}

    public virtual float GetDistanceToDestination() {

        return float.NaN;
    }

    public virtual Vector3 GetDirectionTo( CharacterPawnBase otherPawn ) {

        return Vector3.forward;
    }

	public virtual void MakeDead() {
		
	}
}
