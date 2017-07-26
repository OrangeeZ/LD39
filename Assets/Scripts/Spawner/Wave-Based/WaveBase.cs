using System;
using UnityEngine;
using System.Collections;
using UniRx;

public class WaveBase : MonoBehaviour {

	public GameObject[] Spawners;
	public float Frequency;
	public CharacterInfo characterInfo;
	public WeaponInfo startingWeapon;
	public WaveTriggerBase WaveTrigger;

	// Use this for initialization
	private void Start() {
		WaveTrigger.Initialize();
		WaveTrigger.Subscribe( OnWaveTriggerEvent );
		//WaveTrigger.OnTrigger += WaveTriggerOnOnTrigger;
	}
	private void OnWaveTriggerEvent( object nullObject ) {
		Spawn();
	}

	private void Spawn() {
		Debug.Log( 123124 );
		foreach ( var spawner in Spawners ) {
			var character = characterInfo.GetCharacter( startingPosition: spawner.transform.position );
			//character.ItemsToDrop = default ( ItemInfo ); /* fixit */
			if ( startingWeapon != null ) {
				var weapon = startingWeapon.GetItem();
				character.Inventory.AddItem( weapon );
				weapon.Apply();

				//if ( characterInfo.applyColor ) {

				character.Pawn.SetColor( startingWeapon.color );
				//}
			}
		}
	}

}