using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneSpawner : MonoBehaviour {

	public bool IsDrained { get; private set; }

	private float _autoDrainSpeed = 10f;

	[SerializeField]
	private ZoneInfo _zoneInfo;

	private readonly List<Character> _characters = new List<Character>();

	private float _healingAmount;
	private ZoneView _view;
	private bool _isAutoDrainStarted;

	public void Initialize() {

		_view = Instantiate( _zoneInfo.ZonePrefab, transform.position, transform.rotation ) as ZoneView;

		_view.transform.localScale = _zoneInfo.Size;
		_view.CharacterEntered += OnCharacterEnter;
		_view.CharacterExited += OnCharacterExit;

		_healingAmount = _zoneInfo.HealthPool;

		_isAutoDrainStarted = false;
		IsDrained = false;
	}

	public void StartAutoDrain() {

		_isAutoDrainStarted = true;
	}

	private void Update() {

		if ( _healingAmount <= 0 ) {

			return;
		}

		foreach ( var each in _characters ) {

			var healingAmount = each.Status.ModifierCalculator.CalculateFinalValue( ModifierType.SunHealthRestore, _zoneInfo.HealingSpeed );
			var healingPerDeltaTime = healingAmount * Time.deltaTime;

			each.Heal( healingPerDeltaTime );
			_healingAmount -= healingPerDeltaTime;
		}

		if ( _isAutoDrainStarted ) {

			_healingAmount -= _autoDrainSpeed * Time.deltaTime;
		}

		var rate = _healingAmount / _zoneInfo.HealthPool;
		_view.UpdateIntensity( rate );

		if ( rate <= 0f ) {

			IsDrained = true;

			Cleanup();
		}
	}

	private void Cleanup() {

		Destroy( _view );
		_characters.Clear();
	}

	private void OnCharacterEnter( Character character ) {

		_characters.Add( character );
	}

	private void OnCharacterExit( Character character ) {

		_characters.Remove( character );
	}

}