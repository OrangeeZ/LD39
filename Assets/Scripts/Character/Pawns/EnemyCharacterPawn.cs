using UnityEngine;
using System.Collections;

public class EnemyCharacterPawn : CharacterPawn {

	[SerializeField]
	private NavMeshAgent _navMeshAgent;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private float _fadeSpeed = 0.25f;

	public override void SetSpeed( float newSpeed ) {

		_navMeshAgent.speed = newSpeed;
	}

	public override void SetDestination( Vector3 destination ) {

		_navMeshAgent.SetDestination( destination );
	}

	public override float GetDistanceToDestination() {

		return _navMeshAgent.remainingDistance;
	}

	public override void ClearDestination() {
		
		if ( _navMeshAgent.isActiveAndEnabled ) {

			_navMeshAgent.destination = transform.position;
		}
	}

	public override void MakeDead() {

		base.MakeDead();

		if ( _navMeshAgent.isActiveAndEnabled ) {

			_navMeshAgent.Stop();
		}
	}

	public IEnumerable Fade( bool isOut ) {

		var duration = _fadeSpeed;
		var timer = new AutoTimer( duration );
		var toColor = Color.black;
		toColor.a = 0;

		while ( timer.ValueNormalized < 1 ) {

			_spriteRenderer.color = Color.Lerp( Color.white, toColor, isOut ? 1 - timer.ValueNormalized : timer.ValueNormalized );

			yield return null;
		}
	}

	public void SetPosition( Vector3 position ) {

		if ( _navMeshAgent.isActiveAndEnabled ) {

			_navMeshAgent.Warp( position );
		}

		transform.position = position;
	}

	protected override void Update() {

		base.Update();

		var direction = _navMeshAgent.velocity;
		direction.x *= -1;
		UpdateSpriteAnimationDirection( direction );
	}

}
