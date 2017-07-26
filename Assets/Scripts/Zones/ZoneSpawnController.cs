using UnityEngine;
using System.Collections;

public class ZoneSpawnController : MonoBehaviour {

	[SerializeField]
	private ZoneSpawner[] _zoneSpawners;

	private ZoneSpawner[] _currentZone;

	[SerializeField]
	private float _autoDrainInterval;

	private float _zoneTimer = 0f;

	public int ZonesInTheSameTime;

	public void Initialize() {
		_currentZone = new ZoneSpawner[ZonesInTheSameTime];
		enabled = true;
	}

	private void Awake() {

		enabled = false;
	}

	private void Update() {

		var flagStartAutoDrain = false;
		if ( ( _zoneTimer += Time.deltaTime ) >= _autoDrainInterval ) {
			flagStartAutoDrain = true;
		}

		for (int i = 0; i < ZonesInTheSameTime; i++) {
			if ( _currentZone[i] == null || _currentZone[i].IsDrained ) {
				_currentZone[i] = _zoneSpawners.RandomElement();
				_currentZone[i].Initialize();
				_zoneTimer = 0f;

				if (flagStartAutoDrain) {
					_currentZone [i].StartAutoDrain ();
				}
			}


		}

	}

	[ContextMenu( "Hook zones" )]
	private void HookZones() {

		_zoneSpawners = FindObjectsOfType<ZoneSpawner>();
	}

}