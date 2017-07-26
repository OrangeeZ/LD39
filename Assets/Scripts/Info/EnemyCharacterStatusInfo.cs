using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/Enemy Status Info" )]
public class EnemyCharacterStatusInfo : CharacterStatusInfo {

	public float AggroRadius;
	public RangedWeaponInfo Weapon1;
	public ItemInfo[] ItemsToDrop;
	public EnemyCharacterPawn PawnPrefab;
	public float DropChance;
	public AudioClip[] EnemySpottedSound;

	public float SpawnInterval;
	public float MaxLiveEnemiesPerSpawner;

	public float SpeakChance;
	public string SpeakLineId;

	[Header("Xeno")]
	public float FrightenRadius = 5f;
	public float FrightenChance = .5f;
	public float AutoAggroCheckInterval = 2f;
	public float AutoAggroChance = .1f;
	public bool IsAgressive = false;

	public override void Configure( csv.Values values ) {

		base.Configure( values );

		AggroRadius = values.Get( "AgroRadius", 0 );
		Weapon1 = values.GetScriptableObject<RangedWeaponInfo>( "Weapon1" );
		ItemsToDrop = values.GetScriptableObjects<ItemInfo>( "DroppedItems" );
		DropChance = values.Get( "DropChance", 0f );
		SpawnInterval = values.Get( "RespawnTimer", -1f );
		SpeakChance = values.Get( "SpeakChance", 0.2f );

		MaxLiveEnemiesPerSpawner = values.Get( "MaxRespCount", 1f );
		PawnPrefab = values.GetPrefabWithComponent<EnemyCharacterPawn>( "Visual", fixName: false );
	}

}