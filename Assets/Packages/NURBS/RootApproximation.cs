using System;
using UnityEngine;
using System.Collections;

public static class RootApproximation {

	public static float maxError = 1e-6f;

	public static float Evaluate( System.Func<float, float> f, float from = 0f, float to = 1f ) {
		var midpoint = 0f;

		var safec = 40;

		while ( ( to - from ) > maxError ) {
			if ( safec-- <= 0 ) {
				
				throw new Exception();
			}

			midpoint = ( to + from ) * 0.5f;

			var lastSign = f( midpoint ) > 0 ? 1 : -1f;

			if ( lastSign > 0 ) {
				from = midpoint;
			} else if ( lastSign < 0 ) {
				to = midpoint;
			} else {
				break;
			}
		}

		return midpoint;
	}
}
