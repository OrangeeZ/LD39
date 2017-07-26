using UnityEngine;
using System.Collections;

public class RoomEffectRight : RoomEffect {

	public override void Activate() {

		base.Activate();

		CarStateController.Instance.Speed += EffectValue * Time.deltaTime;
	}

}