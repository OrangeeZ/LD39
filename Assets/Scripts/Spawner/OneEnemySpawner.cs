using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class OneEnemySpawner : SpawnerBase {

	public Action<Character> Spawned;
	public EnemyCharacterInfo characterInfo;
	public EnemyCharacterStatusInfo characterStatusInfo;
	public ItemInfo[] startingItems;

	private float _lastSpawnTime = 0f;
	private bool _isActive = false;

	private List<Character> _characters = new List<Character>();

	public override void Initialize() {

		Spawn();

		_isActive = true;
	}

	private void Spawn() {

		_characters.RemoveAll( where => where.Health.Value <= 0 );

		if ( _characters.Count >= characterStatusInfo.MaxLiveEnemiesPerSpawner ) {

			return;
		}

		var character = characterInfo.GetCharacter( startingPosition: transform.position, replacementStatusInfo: characterStatusInfo );
		_characters.Add( character );

		foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {

			character.Inventory.AddItem( each );
		}

		if ( characterStatusInfo != null ) {

			character.ItemsToDrop = characterStatusInfo.ItemsToDrop;

			character.dropProbability = characterStatusInfo.DropChance;
			character.speakProbability = characterStatusInfo.SpeakChance;

			var weapon = characterStatusInfo.Weapon1.GetItem();
			character.Inventory.AddItem( weapon );

			weapon.Apply();
		}
	}

	private void OnValidate() {

		name = string.Format( "One Enemy Spawner [{0}]", characterStatusInfo == null ? "null" : characterStatusInfo.name );
	}

	private void Update() {

		if ( !_isActive ) {

			return;
		}

		if ( characterStatusInfo.SpawnInterval >= 0 && ( Time.timeSinceLevelLoad - _lastSpawnTime ) >= characterStatusInfo.SpawnInterval ) {

			Spawn();

			_lastSpawnTime = Time.timeSinceLevelLoad;
		}
	}

}