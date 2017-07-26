using System;
using System.Linq;
using Packages.EventSystem;
using UnityEngine;
using UniRx;

public class EnemySpawner : SpawnerBase {

	public class Spawned : IEventBase {

		public Character Character;

	}

	public EnemyCharacterInfo characterInfo;
	public EnemyCharacterStatusInfo characterStatusInfo;

	public float SpawnInterval;
	public int SpawnLimit;

	private float _startTime;
	private int _spawnCount;

	private Character _character;

	[Expressions.CalculatorExpression]
	public StringReactiveProperty Activation;

	[Expressions.CalculatorExpression]
	public StringReactiveProperty Deactivation;

	private void OnValidate() {

		name = string.Format( "Spawner [{0}]", characterStatusInfo.name );
	}

	public override void Initialize() {

		Spawn();
	}

	public Character GetLastSpawnedCharacter() {

		return _character;
	}

	private void Spawn() {

		_startTime = 0.0f;

		//if ( SpawnLimit > 0 && _spawnCount >= SpawnLimit ) {

		//	return;
		//}
		_spawnCount += 1;

		_character = characterInfo.GetCharacter( startingPosition: transform.position, replacementStatusInfo: characterStatusInfo );

		if ( characterStatusInfo != null ) {
			_character.ItemsToDrop = characterStatusInfo.ItemsToDrop;
			_character.dropProbability = characterStatusInfo.DropChance;
			_character.speakProbability = characterStatusInfo.SpeakChance;

			var weapon = characterStatusInfo.Weapon1.GetItem();
			_character.Inventory.AddItem( weapon );
			weapon.Apply();
		}

		EventSystem.RaiseEvent( new Spawned {Character = _character} );

	}

}