using UnityEngine;
using System.Collections;

public class NPCSpawner : AObject {

	public NPCListInfo NpcListInfo;
	public NPCInfo OverriderNpcInfo;

	private NPCInfo _currentInfo;
	private NPCView _view;

	private float _currentRechargeTimer;

	private void Start() {

		_currentInfo = OverriderNpcInfo == null ? NpcListInfo.Infos.RandomElement() : OverriderNpcInfo;

		Spawn();
	}

	private void Spawn() {

		_view = Instantiate( _currentInfo.groundView, position: this.position );
		_view.npc = _currentInfo;

		_currentRechargeTimer = 0f;
	}

	private void Update() {

		if ( _view != null || _currentInfo.RechargeTimer.IsNan() ) {

			return;
		}

		if ( ( _currentRechargeTimer += Time.deltaTime ) >= _currentInfo.RechargeTimer ) {
			
			Spawn();
		}
	}

	private void OnValidate() {

		name = string.Format( "NPC Spawner [{0}]", _currentInfo == null ? "null" : _currentInfo.name );
	}

}