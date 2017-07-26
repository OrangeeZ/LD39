using UnityEngine;
using System.Collections;

public class TimedDebuffApplier : MonoBehaviour {

	//public CarStateDebuff[] Debuffs;
	public CarStateController Target;

	public float Interval = 3f;

	IEnumerator Start() {

		while ( true ) {
			
			yield return new WaitForSeconds( Interval );

			//Debuffs.RandomElement().ApplyTo( Target );
		}
	}

}
