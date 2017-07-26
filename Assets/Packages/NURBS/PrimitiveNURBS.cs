using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PrimitiveNURBS {
	[SerializeField]
	private int degree = 2;

	[SerializeField]
	private Vector3[] points = null;

	[SerializeField]
	private float[] weights = null;

	[SerializeField]
	private float[] knotVector = null;

	[SerializeField]
	private bool isClosed = false;

	private float[] basisFunctionBuffer = null;

	public PrimitiveNURBS( int degree, Vector3[] points, float[] weights, bool isClosed ) {
		this.degree = degree;
		this.points = points;
		this.weights = weights;
		this.isClosed = isClosed;

		if ( isClosed && points.Length > degree ) {
			var newPoints = points.ToList();
			var newWeights = weights.ToList();

			for ( var i = 0; i < degree; ++i ) {
				newPoints.Insert( i, points[points.Length - degree + i] );
				newWeights.Insert( i, weights[points.Length - degree + i] );
			}

			this.points = newPoints.ToArray();
			this.weights = newWeights.ToArray();
		}

		knotVector = new float[this.points.Length + degree + ( isClosed ? 1 : 1 )];

		for ( var i = 0; i < knotVector.Length; ++i ) {
			knotVector[i] = ( (float)i ) / ( knotVector.Length - 1 );
		}
	}

	float f( int i, int p, float t ) {
		return ( t - knotVector[i] ) / ( knotVector[i + p] - knotVector[i] );
	}

	float g( int i, int p, float t ) {
		return ( knotVector[i + p + 1] - t ) / ( knotVector[i + p + 1] - knotVector[i + 1] );
	}

	float[] BasisFunctionValues( int k, int p, float t ) {
		if ( basisFunctionBuffer == null ) {
			basisFunctionBuffer = new float[p + 1];
		}

		basisFunctionBuffer[0] = 1;
		for ( var i = 1; i < basisFunctionBuffer.Length; ++i ) {
			basisFunctionBuffer[i] = 0;
		}

		var m = ( t - knotVector[k] ) / ( knotVector[k + 1] - knotVector[k] );

		for ( var i = 1; i <= p; ++i ) {
			for ( var j = i - 1; j >= 0; --j ) {
				var tmp = basisFunctionBuffer[j] * ( m + j ) / i;

				basisFunctionBuffer[j + 1] += basisFunctionBuffer[j] - tmp;
				basisFunctionBuffer[j] = tmp;
			}
		}

		return basisFunctionBuffer;
	}

	float BasisFunction( int i, int p, float t ) {
		if ( p == 0 ) {
			if ( t >= knotVector[i] && t < knotVector[i + 1] )
				return 1;
			else
				return 0;
		}

		return f( i, p, t ) * BasisFunction( i, p - 1, t ) + g( i, p, t ) * BasisFunction( i + 1, p - 1, t );
	}

	private float Clamp( float t ) {
		var shift = isClosed ? 0 : 1;
		var leftBound = degree - shift;
		var rightBound = points.Length + shift;

		return Mathf.Lerp( knotVector[leftBound], knotVector[rightBound], t );
	}

	private int CalculateSpan( float x ) {
		var n = knotVector.Length - degree - 1;

		if ( x == knotVector[n + 1] ) {
			return n;
		}

		var left = degree - 1;
		var right = n + 1;
		var mid = ( left + right ) / 2;

		var refc = 20;

		while ( x < knotVector[mid] || x >= knotVector[mid + 1] ) {
			if ( --refc < 0 ) {
				Debug.Log( x );
				break;
			}

			if ( x < knotVector[mid] ) {
				right = mid;
			} else {
				left = mid;
			}

			mid = ( left + right ) / 2;
		}

		return mid;
	}

	public Vector3 Evaluate( float t ) {
		t = Clamp( t );

		var result = Vector3.zero;
		var normalizingFactor = 0f;

		var knotSpan = CalculateSpan( t );

		var basisFunctionValues = BasisFunctionValues( knotSpan, degree, t );

		for ( var i = 0; i <= degree; ++i ) {
			var index = Mathf.Clamp( knotSpan - i, 0, points.Length - 1 );

			normalizingFactor += basisFunctionValues[i] *= weights[index];
			result += basisFunctionValues[i] * points[index];
		}

		return result / normalizingFactor;
	}

	float BasisFunctionDerivative( int i, int n, float t ) {
		return ( n / ( knotVector[i + n] - knotVector[i] ) ) * BasisFunction( i, n - 1, t ) -
			   ( n / ( knotVector[i + n + 1] - knotVector[i + 1] ) ) * BasisFunction( i + 1, n - 1, t );
	}

	private float BasisFunctionDerivativeAlt( int k, int p, float t ) {
		var m = 1f / ( knotVector[k + 1] - knotVector[k] );

		return m * BasisFunction( k, p - 1, t ) -
			   m * BasisFunction( k + 1, p - 1, t );
	}

	public Vector3 EvaluateDerivative( float t ) {
		t = Clamp( t );

		var normalizingFactor = 0f;

		var derivative = Vector3.zero;

		var result1 = Vector3.zero;
		var result3 = Vector3.zero;
		var result2 = 0f;
		var result4 = 0f;

		var knotSpan = CalculateSpan( t );

		var basisFunctionValues = BasisFunctionValues( knotSpan, degree, t );

		for ( var i = 0; i <= degree; ++i ) {
			var index = Mathf.Clamp( knotSpan - i, 0, points.Length - 1 );

			result2 += basisFunctionValues[i];
			result3 += basisFunctionValues[i] * points[index];

			normalizingFactor += basisFunctionValues[i] *= weights[index];
		}

		for ( var i = knotSpan - 1; i >= 0; --i ) {
			var basisFunctionDerivativeAtI = weights[i] * BasisFunctionDerivative( i, degree, t );

			if ( basisFunctionDerivativeAtI == 0f ) {
				break;
			}

			result1 += points[i] * basisFunctionDerivativeAtI;
			result4 += basisFunctionDerivativeAtI;
		}

		for ( var i = knotSpan; i < points.Length; ++i ) {


			var basisFunctionDerivativeAtI = weights[i] * BasisFunctionDerivative( i, degree, t );

			if ( basisFunctionDerivativeAtI == 0f ) {
				break;
			}


			result1 += points[i] * basisFunctionDerivativeAtI;

			result4 += basisFunctionDerivativeAtI;
		}

		normalizingFactor *= normalizingFactor;

		derivative = ( result1 * result2 );
		derivative -= ( result3 * result4 );

		derivative /= normalizingFactor;

		return derivative;
	}
}