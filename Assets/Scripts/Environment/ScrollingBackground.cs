using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

	public GameObject[] Targets;

	public AnimationCurve RotationSpeedCurve;
	public float SpeedScale = 0.5f;

	void Update() {

		for ( int i = 0; i < Targets.Length; i++ ) {

			var distanceRatio = (float) i / ( Targets.Length - 1 );

			Targets[i].transform.rotation *= Quaternion.AngleAxis( RotationSpeedCurve.Evaluate(distanceRatio) * SpeedScale * CarStateController.Instance.Speed * Time.deltaTime, Vector3.forward );
		}
	}

}
