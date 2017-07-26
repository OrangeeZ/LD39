using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class AdaptiveChebyshevApproximation {

	public Func<float, float, float> f = null;

	public float from = 0f;
	public float to = 0f;

	public bool isPiecewise = false;

	private int desiredDegree = 4; //4
	private int maxDegree = 5; //8

    [SerializeField]
	private float maxError = 1e-4f; //0.00001f;
    
    [SerializeField]
    private int maxLevel = 7; //6

	[SerializeField]
	private List<ChebyshevApproximation> approximations = null;

    [SerializeField]
	private float[] chebyshevCoefficientsCache = null;
    
    [SerializeField]
    private float[] functionCache = null;

	private float[] CalculateChebyshefCoefficients( int degree, float from, float to ) {

		var functionRangeLength = to - from;

		var accumulatedValue = isPiecewise ? Evaluate( from ) : 0f;

		for ( var j = 0; j < degree; ++j ) {
			var piK = Mathf.PI * ( j + 0.5f ) / degree;

			functionCache[j] = accumulatedValue + f( from, from + ( (Mathf.Cos( piK ) + 1f) * 0.5f * functionRangeLength ) );
		}

		for ( var i = 0; i < degree; ++i ) {

			var sum = 0f;
			for ( var j = 0; j < degree; ++j ) {
				var piK = Mathf.PI * ( j + 0.5f ) / degree;

				sum += functionCache[j] * Mathf.Cos( piK * i );
			}

			chebyshevCoefficientsCache[i] = sum * 2f / degree;
		}

		return chebyshevCoefficientsCache;
	}

	private void SubdivideSegment( float from, float to, int level ) {
		//Profiler.BeginSample( "Approximation" );
		var coefficients = CalculateChebyshefCoefficients( maxDegree, from, to );
		var error = 0f;

		for ( var i = desiredDegree; i < maxDegree; ++i ) {
			error += Mathf.Abs( coefficients[i] );
		}

		if ( error < maxError || level > maxLevel ) {
			//Profiler.BeginSample( "Compose" );
			var approximation = new ChebyshevApproximation();
			approximation.CopyCoefficients( f, from, to, desiredDegree, coefficients, 0, desiredDegree );
			//Profiler.EndSample();

			//Profiler.BeginSample( "Insertion" );
			approximations.Add( approximation );
			//Profiler.EndSample();
		} else {
			var mid = ( from + to ) * 0.5f;

			SubdivideSegment( from, mid, level + 1 );
			SubdivideSegment( mid, to, level + 1 );
		}
		//Profiler.EndSample();
	}

	public void CalculateApproximation() {

		chebyshevCoefficientsCache = new float[maxDegree];
		functionCache = new float[maxDegree];

		approximations = new List<ChebyshevApproximation>( capacity: (int)Mathf.Pow( 2, maxLevel + 1 ) );

		SubdivideSegment( from, to, 0 );
		chebyshevCoefficientsCache = null;
		functionCache = null;

		//Debug.Log( approximations.Count );
		//Debug.Break();
	}

	private int GetIntervalIndex( int pivot, float t ) {

		var left = 0;
		var right = approximations.Count;

		var approximation = approximations[pivot];

		if ( approximation.from > t ) {

			right = pivot;
		} else if ( approximation.to < t ) {

			left = pivot;
		} else {

			return pivot;
		}

		var middle = ( left + right ) / 2;

		while ( approximations[middle].from > t || approximations[middle].to < t ) {
			
			if ( approximations[middle].from > t ) {

				right = middle;
			} else if ( approximations[middle].to < t ) {

				left = middle;
			}

			middle = ( right + left ) / 2;
		}

		return middle;
	}
	
	public float Evaluate( float t ) {

		if ( approximations.IsEmpty() ) {

			return 0f;
		}

		var intervalIndex = GetIntervalIndex( 0, t );

		var currentApproximation = approximations[intervalIndex];

		var remappedValue = ( t - currentApproximation.from ) / ( currentApproximation.to - currentApproximation.from );

		return currentApproximation.Evaluate( remappedValue );
	}

	public float Evaluate( ref int pivot, float t ) {

		if ( approximations.IsEmpty() ) {

			return 0f;
		}

		pivot = GetIntervalIndex( pivot, t );

		var currentApproximation = approximations[pivot];

		var remappedValue = ( t - currentApproximation.from ) / ( currentApproximation.to - currentApproximation.from );

		return currentApproximation.Evaluate( remappedValue );
	}
}
