using UnityEngine;
using System.Collections;

public class ExternalEnemy : MonoBehaviour {

	public int AttackCount;
	public int Damage;
	public float AttackInterval;
	public RoomDevice AttackTarget;
	public ExternalEnemyController Controller;
	public ExternalEnemyInfo EnemyInfo { get; set; }

	public virtual void Initialize() {

		StartCoroutine( AttackLoop() );
	}

	public void Destroy() {

		Controller.RemoveEnemy( this );
	}

	private IEnumerator Approach() {

		var timer = new AutoTimer( 2f );
		var initialPosition = transform.position;
		var targetPosition = initialPosition.Set( x: AttackTarget.transform.position.x + Random.Range( -2f, 2f ) );

		while ( timer.ValueNormalized < 1f ) {

			transform.position = Vector3.Lerp( initialPosition, targetPosition, Mathf.Sqrt( timer.ValueNormalized ) );

			yield return null;
		}
	}

	private IEnumerator AttackLoop() {

		yield return StartCoroutine( Approach() );

		for ( var i = 0; i < AttackCount; ++i ) {

			AttackTarget.Damage( Damage );

			yield return new WaitForSeconds( AttackInterval );
		}

		Destroy();
	}

}