using UnityEngine;
using System.Collections;

public class CarStateController : MonoBehaviour {

	public static CarStateController Instance { get; private set; }

	public float SpeedNormalized {
		get { return Speed / GlobalGameInfo.MaxSpeed; }
	}

	public float DistanceNormalized {
		get { return Speed / 500; }
	}

	public float Speed;
	public float Distance;

	public GlobalGameInfo GlobalGameInfo;

	private void Awake() {

		Instance = this;

		Speed = GlobalGameInfo.StartSpeed;
	}

	private void Update() {

		Speed -= GlobalGameInfo.GlobalSpeedLow * Time.deltaTime;

		Distance += ( Speed / 60f ) * Time.deltaTime;

		Speed = Speed.Clamped( 0, GlobalGameInfo.MaxSpeed );
	}

}