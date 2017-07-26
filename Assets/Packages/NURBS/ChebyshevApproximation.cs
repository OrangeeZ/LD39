using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public struct ChebyshevApproximation {

	[SerializeField]
	public float[] coefficients;

    [SerializeField]
	public int degree;

	[SerializeField]
	private float? cachedEnd;

	public float from;
	public float to;

	public void CalculateCoefficients( Func<float, float, float> f, float from, float to, int degree ) {

		this.degree = degree;
		this.from = from;
		this.to = to;

		coefficients = new float[degree];

		var functionRangeLength = to - from;

		var jCache = new float[degree];
		for ( var j = 1f; j <= degree; ++j ) {
			var piK = Mathf.PI * ( j - 0.5f ) / degree;

			jCache[(int)j - 1] = f( from, from + ( Mathf.Cos( piK ) * functionRangeLength ) );
		}

		for ( var i = 0; i < degree; ++i ) {
			coefficients[i] = 2f / degree;

			var cosineSum = 0f;
			for ( var j = 1f; j <= degree; ++j ) {
				var piK = Mathf.PI * ( j - 0.5f ) / degree;

				cosineSum += jCache[(int)j - 1] * Mathf.Cos( piK * i );
			}

			coefficients[i] *= cosineSum;
		}

		//cachedValue = EvaluateInternal( 1f );
	}

	public void CopyCoefficients( Func<float, float, float> f, float from, float to, int degree, float[] sourceCoefficients, int startIndex, int endIndex ) {

		this.degree = degree;
		this.coefficients = new float[degree];
		this.from = from;
		this.to = to;

		Array.Copy( sourceCoefficients, startIndex, this.coefficients, startIndex, endIndex - startIndex  );
	}

	private float EvaluateInternal( float x ) {

	    if ( degree <= 0 ) {
	        
            return 0f;
	    }

		x = x * 2f - 1;

		var result = 0f;
		var aCos = Mathf.Acos ( x );

		for ( var i = 0; i < degree; ++i ) {
			result += coefficients[i] * Mathf.Cos( i * aCos );
		}

		result -= 0.5f * coefficients[0];

		return result;
	}

	public float Evaluate( float x ) {

		if ( x == 1f ) {

			if ( cachedEnd == null ) {

				cachedEnd = EvaluateInternal( 1f );
			}

			return cachedEnd.Value;
		}

		return EvaluateInternal( x );
	}
}
