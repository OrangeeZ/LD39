using UnityEngine;
using System.Collections;

public class ExternalEnemyHitZone : MonoBehaviour {

	void OnTriggerEnter( Collider other ) {

		var enemy = other.GetComponent<ExternalEnemy>();
		if ( enemy != null ) {
			
			enemy.Destroy();

			DestroyImmediate( gameObject );
		}
	}
}
