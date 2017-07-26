using UnityEngine;
using System.Collections;

public class RoomEffectEngine : RoomEffect {

	public override void Activate() {

		base.Activate();

		CarStateController.Instance.Speed += EffectValue;
	}
}
