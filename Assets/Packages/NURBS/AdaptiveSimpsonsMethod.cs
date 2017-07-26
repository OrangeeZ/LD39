using UnityEngine;
using System.Collections;

public class AdaptiveSimpsonsMethod {
	public float maxError = 0.0000001f;
	public int maxRecursionDepth = 9;

	private float EvaluateRecursive( System.Func<float, float> f, float from, float to, float maxError, float wholeArcValue, float fA, float fB, float fC, int depth ) {
		var midpoint = ( from + to ) * 0.5f;
		var leftArcMidpoint = ( from + midpoint ) * 0.5f;
		var rightArcMidpoint = ( to + midpoint ) * 0.5f;

		var fLeftMidpoint = f( leftArcMidpoint );
		var fRightMidpoint = f( rightArcMidpoint );

		var h = ( to - from ) / 12f;
		var leftArcValue = h * ( fA + 4f * fLeftMidpoint + fC );
		var rightArcValue = h * ( fC + 4f * fRightMidpoint + fB );

		var arcSum = leftArcValue + rightArcValue;

		if ( Mathf.Abs( arcSum - wholeArcValue ) <= maxError * 15f || depth == 0 )
			return arcSum + ( arcSum - wholeArcValue ) / 15f;

		return EvaluateRecursive( f, from, midpoint, maxError * 0.5f, leftArcValue, fA, fC, fLeftMidpoint, depth - 1 ) +
			   EvaluateRecursive( f, midpoint, to, maxError * 0.5f, rightArcValue, fC, fB, fRightMidpoint, depth - 1 );
	}

	public float Evaluate( System.Func<float, float> f, float from, float to ) {
		var fA = f( from );
		var fB = f( to );
		var fC = f( ( from + to ) * 0.5f );

		var wholeArcValue = ( to - from ) / 6f * ( fA + 4f * fC + fB );

		return EvaluateRecursive( f, from, to, maxError, wholeArcValue, fA, fB, fC, maxRecursionDepth );
	}
}
