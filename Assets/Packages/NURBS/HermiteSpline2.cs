using UnityEngine;
using System.Collections;

public class HermiteSpline2 : MonoBehaviour {

	public Vector3 p1 = Vector3.zero;
	public Vector3 t1 = Vector3.right;

	public Vector3 p2 = Vector3.one * 10f;
	public Vector3 t2 = Vector3.right;

	private Vector3 Evaluate( float s ) {
		var h1 = 2 * Mathf.Pow(s, 3f) - 3 * Mathf.Pow(s, 2) + 1;
		var h2 = -2 * Mathf.Pow(s, 3) + 3 * Mathf.Pow(s, 2);
		var h3 = Mathf.Pow(s, 3) - 2 * Mathf.Pow(s, 2) + s;
		var h4 = Mathf.Pow(s, 3) -  Mathf.Pow(s, 2);

		return h1 * p1 + 
			   h2 * p2 + 
			   t1 * h3 + 
			   t2 * h4;
	}

	void OnDrawGizmos() {
		for ( var i = 0f; i <= 1f; i += 0.05f ) {
			Gizmos.DrawLine( Evaluate( i ), Evaluate( i + 0.025f ) );
		}
	}
}
