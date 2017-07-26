using UnityEngine;
using System.Collections;
using csv;

public class GlobalGameInfo : ScriptableObject, ICsvConfigurable {

	public float MaxSpeed;
	public float StartSpeed;
	public float GlobalSpeedLow;

	public float EnterpriseSpeedStart;
	public float EnterpriseSpeedStop;

	public float HeroSpeed;
	public float JumpHeight;
	public float RepairCount;

	public void Configure( Values values ) {

		values.Get( "MaxSpeed", out MaxSpeed );
		values.Get( "StartSpeed", out StartSpeed );
		values.Get( "GlobalSpeedLow", out GlobalSpeedLow );

		values.Get( "EnterpriseSpeedEnable", out EnterpriseSpeedStart );
		values.Get( "EnterpriseSpeedDisable", out EnterpriseSpeedStop );

		values.Get( "HeroSpeed", out HeroSpeed );
		values.Get( "Jump", out JumpHeight );
		values.Get( "RepairCount", out RepairCount );
	}

}