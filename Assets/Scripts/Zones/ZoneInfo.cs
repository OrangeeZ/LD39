using UnityEngine;
using System.Collections;
using csv;

public class ZoneInfo : ScriptableObject, ICsvConfigurable {

	public float HealthPool;
	public float HealingSpeed;
	public Vector3 Size;

	public ZoneView ZonePrefab;

	public void Configure( Values values ) {

		HealingSpeed = values.Get( "HpPerSec", 0f );
		HealthPool = values.Get( "HpPool", 0f );
		ZonePrefab = values.GetPrefabWithComponent<ZoneView>( "Visual", fixName: false );

		var sizes = values.Get( "HitBox", "1x1" ).Split( 'x' );
		var x = float.Parse( sizes[0] );
		var z = float.Parse( sizes[1] );

		Size = new Vector3( x, 1, z );
	}

}