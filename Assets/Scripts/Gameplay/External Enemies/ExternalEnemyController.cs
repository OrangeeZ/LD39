using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExternalEnemyController : MonoBehaviour {

	[SerializeField]
	private ExternalEnemyInfo _enemyInfo;

	[SerializeField]
	private RoomDevice _roomDevice;
	
	[SerializeField]
	private ExternalEnemy[] _enemyPrefabs;

	private List<ExternalEnemy> _spawnedEnemies;

	private float _spawnInterval = 16f;

	IEnumerator Start() {

		_spawnedEnemies = new List<ExternalEnemy>();

		var nextSpawnDistance = _enemyInfo.SpawnRange;

		while ( true ) {
			
			yield return new WaitUntil( () => nextSpawnDistance < CarStateController.Instance.Distance );

			nextSpawnDistance += _enemyInfo.SpawnRange;

			var enemyInstance = Instantiate( _enemyPrefabs.RandomElement(), transform.position, Quaternion.identity ) as ExternalEnemy;

			enemyInstance.AttackCount = _enemyInfo.Type == ExternalEnemyType.Permanent ? int.MaxValue : 1;
			enemyInstance.AttackInterval = _enemyInfo.AttackCooldown;
			enemyInstance.EnemyInfo = _enemyInfo;
			enemyInstance.Damage = _enemyInfo.Damage;
			enemyInstance.AttackTarget = _roomDevice;
            enemyInstance.Controller = this;

			enemyInstance.Initialize();

			_spawnedEnemies.Add( enemyInstance );
		}
	}

	public void RemoveEnemy( ExternalEnemy enemy ) {

		_spawnedEnemies.Remove( enemy );

		Destroy( enemy.gameObject );
	}
}
