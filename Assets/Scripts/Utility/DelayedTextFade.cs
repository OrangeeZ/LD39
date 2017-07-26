using UnityEngine;
using System.Collections;

public class DelayedTextFade : MonoBehaviour {

	public float Delay;
	public float Duration;

	public TextMesh TextMesh;

	// Use this for initialization
	IEnumerator Start () {

		yield return new WaitForSeconds( Delay );

		var from = TextMesh.color;
		var to = TextMesh.color;
		to.a = 0;

		var timer = new AutoTimer( Duration );
		while ( timer.ValueNormalized < 1f ) {

			TextMesh.color = Color.Lerp( from, to, timer.ValueNormalized );

			yield return null;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
