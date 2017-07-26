using UnityEngine;
using System.Collections;
using Packages.EventSystem;
using UniRx;

public class DamageIndicatorController : MonoBehaviour {

	[SerializeField]
	private int _targetTeamId;

	[SerializeField]
	private DamageIndicator _damageIndicator;

	private void Start() {

		EventSystem.Events.SubscribeOfType<Character.RecievedDamage>( OnCharacterReceiveDamage );
	}

	private void OnCharacterReceiveDamage( Character.RecievedDamage recievedDamage ) {

		if ( Camera.main == null || recievedDamage.Character.Pawn == null ) {

			return;
		}
		
		var instance = Instantiate( _damageIndicator );
		instance.transform.position = Camera.main.WorldToScreenPoint( recievedDamage.Character.Pawn.position );
		instance.transform.SetParent( transform );
		instance.Initialize( recievedDamage.Damage, recievedDamage.Character.IsEnemy() );
	}

}