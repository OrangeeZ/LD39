using System;
using System.Linq;
using UnityEngine;
using UniRx;

public class BossSpawner : MonoBehaviour {

	public Action<Character> Spawned;

	public CharacterInfo characterInfo;

	public ItemInfo[] startingItems;

	public WeaponInfo startingWeapon;

	public CameraBehaviour cameraBehaviour;

    public ItemInfo itemToDrop;
    public float dropProbability = 0.15f;
	
	private Character character;
		
	[Expressions.CalculatorExpression]
	public StringReactiveProperty activation;
	private Expressions.ReactiveCalculator _reactCalc;

    [Expressions.CalculatorExpression]
    public StringReactiveProperty deactivation;
	private Expressions.ReactiveCalculator _reactCalcDeact;

	private bool wasSpawned;

	private void Start() {
		_reactCalc = new Expressions.ReactiveCalculator (activation);

		_reactCalcDeact = new Expressions.ReactiveCalculator (deactivation);
		
		wasSpawned = false;
	}

	private void SpawnThisEnemyNow11111() {
		if (wasSpawned) {
			return;
		}

		if (_reactCalc.Result.Value < 0) {
			return;
		}

		if (_reactCalcDeact.Result.Value >= 0) {
			return;
		}

		wasSpawned = true;

		character = characterInfo.GetCharacter( startingPosition: transform.position );
		
		foreach ( var each in startingItems.Select( _ => _.GetItem() ) ) {
			character.Inventory.AddItem( each );
		}

	    //character.ItemsToDrop = itemToDrop;
	    character.dropProbability = dropProbability;
		character.speakProbability = 1f;

		if ( startingWeapon != null ) {
			var weapon = startingWeapon.GetItem();
			character.Inventory.AddItem( weapon );
			weapon.Apply();

		    //if ( characterInfo.applyColor ) {
		        
                character.Pawn.SetColor( startingWeapon.color );
		    //}
		}
		
		if ( cameraBehaviour != null ) {
			var cameraBehaviourInstance = Instantiate( cameraBehaviour );
			cameraBehaviourInstance.transform.position = transform.position;
			cameraBehaviourInstance.SetTarget( character.Pawn );
		}

	}

	private void Update() {
		SpawnThisEnemyNow11111 ();
	}
}
