using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class GameplayController : MonoBehaviour {
	
	[SerializeField]
	private PlayerCharacterSpawner _playerSpawner;

    public PlayerCharacterSpawner PlayerSpawner
    {
        get { return _playerSpawner;}
    }

    [SerializeField]
	private SpawnerBase[] _enemySpawners;

	[SerializeField]
	private ZoneSpawnController _zoneSpawnController;

    public static GameplayController Instance { get; private set; }

	void Awake() {

		Instance = this;
	}

	public IEnumerator Start() {

		yield return null;

		Room.InitializeAll();

		_playerSpawner.Initialize();

		foreach ( var each in _enemySpawners ) {
			each.Initialize();
		}

		if ( _zoneSpawnController != null ) {

			_zoneSpawnController.Initialize();
		}
	}

	[ContextMenu("Hook dependencies")]
	private void HookDependencies() {

		_playerSpawner = FindObjectOfType<PlayerCharacterSpawner>();
		_enemySpawners = FindObjectsOfType<SpawnerBase>();
	}
}