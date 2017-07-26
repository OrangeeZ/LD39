using UnityEngine;
using System.Collections;

public class RoomEffectWheel : RoomEffect {

	public override void Activate() {

		base.Activate();

		CarStateController.Instance.Speed += EffectValue * Time.deltaTime;
	}

}