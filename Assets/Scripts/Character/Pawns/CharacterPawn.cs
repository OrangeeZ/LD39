using UniRx;
using UnityEngine;

public class CharacterPawn : CharacterPawnBase {

	public bool canFollowDestination;

	[SerializeField]
	private float _gunYOffset = 0.5f;

	[SerializeField]
	private float _weight = 1f;

	[SerializeField]
	private float _isGroundedChangeDelay = 0.2f;

	[SerializeField]
	private float _rotationToDirectionSpeed = 100;

	[SerializeField]
	private SimpleSphereCollider _sphereCollider;

	[SerializeField]
	private CharacterController _characterController;

	private Vector3? _destination;

	private Transform _turretTarget;

	[SerializeField]
	private Transform _subTransform;

	[SerializeField]
	private WarFogTracer _warFogTracer;

	[SerializeField]
	private CharacterSpriteAnimationController _spriteAnimationController;

	private float _lastGroundedTime = 0f;

	protected virtual void Update() {

		if ( _characterController != null ) {

			_characterController.Move( Vector3.down * Time.deltaTime * _weight );

			if ( _characterController.isGrounded ) {

				_lastGroundedTime = Time.time;
			}
		}
	}

	public Vector3 GetWeaponPosition() {

		return position + Vector3.up * _gunYOffset;
	}

	public override void MoveHorizontal( Vector3 direction ) {

		var directionDelta = direction * speed * Time.deltaTime;

		if ( _characterController == null ) {

			position += directionDelta;
		} else {

			_characterController.Move( directionDelta );
		}

		UpdateSpriteAnimationDirection( direction );
	}

	public void MoveVertical( ref float impulse, float deltaTime ) {

		impulse += Physics.gravity.y * _weight * deltaTime;
		var delta = impulse * Vector3.up;

		if ( _characterController == null ) {

			position += delta * deltaTime;
		} else {

			_characterController.Move( delta * deltaTime );
		}
	}

	public override void SetDestination( Vector3 destination ) {

		_destination = destination;
	}

	public override float GetDistanceToDestination() {

		return _destination.HasValue ? Vector3.Distance( position, _destination.Value ) : float.NaN;
	}

	public override Vector3 GetDirectionTo( CharacterPawnBase otherPawn ) {

		return ( otherPawn.position - position ).normalized;
	}

	public virtual void ClearDestination() {

		_destination = null;
	}

	public void SetColor( Color baseColor ) {

		var renderers = GetComponentsInChildren<Renderer>();
		foreach ( var each in renderers ) {

			each.material.SetColor( "_Color", baseColor );
		}
	}

	public void SetActive( bool isActive ) {

		enabled = isActive;
		gameObject.SetActive( isActive );
	}

	public override void MakeDead() {

		GetSphereSensor().enabled = false;
		GetComponent<Collider>().enabled = false;
	}

	public void UpdateSpriteAnimationDirection( Vector3 direction ) {

		if ( _spriteAnimationController == null ) {

			return;
		}

		var directionX = (int) Mathf.Clamp( -direction.x * 100, -1, 1 );
		var directionY = (int) Mathf.Clamp( -direction.z * 100, -1, 1 );

		_spriteAnimationController.UpdateDirection( directionX, directionY );
	}

	public bool IsGrounded() {

		return Time.time - _lastGroundedTime < _isGroundedChangeDelay;
	}

}