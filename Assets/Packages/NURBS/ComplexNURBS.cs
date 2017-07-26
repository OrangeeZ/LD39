using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR

public partial class ComplexNURBS {

    public bool drawDottedLine = false;

    public Color curveColor = Color.green;

    [ContextMenu( "Flatten Z" )]
    public void FlattenZ() {

        var averageZ = controlPoints.Average( each => each.position.z );

        foreach ( var each in controlPoints ) {

            each.position.z = averageZ;
            each.tangent.z = 0;
        }

        Recalculate();
    }

    [ContextMenu( "Reset Z" )]
    public void ResetZ() {

        foreach ( var each in controlPoints ) {

            each.position.z = 0;
            each.tangent.z = 0;
        }

        Recalculate();
    }

    public void OnDrawGizmos() {

        Gizmos.color = curveColor;

        DrawLousyCurve( Gizmos.DrawLine );
    }

    private void DrawLousyCurve( Action<Vector3, Vector3> drawingDelegate ) {

        if ( controlPoints.IsNullOrEmpty() ) {

            return;
        }

        var step = 2f;

        for ( var i = 0f; i < length; i += step ) {

            drawingDelegate( CalculatePointByDistance( i ), CalculatePointByDistance( i + step ) );
        }
    }

    private void DrawDetailedCurve() {

        for ( var i = 0f; i <= 1f; i += 0.02f ) {

            var from = CalculatePointByDistance( i * length );

            var curveRotation = CalculateRotationByDistance( i * length );

            Gizmos.color = Color.red;

            Gizmos.DrawLine( from, from + curveRotation * Vector3.right );

            Gizmos.color = Color.blue;

            Gizmos.DrawLine( from, from + curveRotation * Vector3.up );

            Gizmos.color = Color.green;

            Gizmos.DrawLine( from, from + curveRotation * Vector3.forward );
        }
    }

}
#endif

[ExecuteInEditMode]
public partial class ComplexNURBS : AObject {

    [Serializable]
    public class ControlPoint {

        public Vector3 position;
        public Vector3 tangent = Vector3.right;
        public float weight;

#if UNITY_EDITOR

        public void Copy( ControlPoint from ) {

            position = from.position;
            tangent = from.tangent;
            weight = from.weight;
        }

        public bool showInInspector = false;

        public override string ToString() {

            return "ControlPoint: " + position.x + ";" + position.y + ";" + position.z + ";" + tangent.x + ";" +
                   tangent.y + ";" + tangent.z + ";" + weight;
        }

        public void FromString( string fromString ) {

            if ( !fromString.Contains( "ControlPoint" ) ) {

                return;
            }

            var tokens = fromString.Replace( "ControlPoint:", "" ).Split( ';' );

            position.x = float.Parse( tokens[0] );
            position.y = float.Parse( tokens[1] );
            position.z = float.Parse( tokens[2] );

            tangent.x = float.Parse( tokens[3] );
            tangent.x = float.Parse( tokens[4] );
            tangent.x = float.Parse( tokens[5] );

            weight = float.Parse( tokens[6] );
        }
#endif
    }

    public List<ControlPoint> controlPoints = new List<ControlPoint>();

    public int degree = 2;
    public float length = 0f;

    //public PrimitiveNURBS curve = null;

    public HermiteSpline curve = null;

    public AdaptiveChebyshevApproximation distanceToParameterApproximation = null;
    public AdaptiveChebyshevApproximation parameterToDistanceApproximation;

    public Bounds bounds;

    public bool isInverted = false;

    //public bool drawDetailedCurve = false;

    public bool isClosed = false;

    //void Start() {
    //	Recalculate();
    //}

    private Bounds CalculateBounds() {
        var result = new Bounds( transform.position, Vector3.zero );

        var center = controlPoints.Aggregate( Vector3.zero, ( current, each ) => current + each.position );

        center *= 1f / controlPoints.Count;
        result.center = center;

        foreach ( var each in controlPoints ) {
            result.Encapsulate( each.position );
        }

        return result;
    }

    public ControlPoint AddControlPoint( Vector3 position, float weight = 1f ) {

        var controlPoint = new ControlPoint {

            position = transform.InverseTransformPoint( position ),
            weight = weight
        };

        controlPoints.Add( controlPoint );

        CreateCurve();

        return controlPoint;
    }

    public ControlPoint InsertControlPointBefore( ControlPoint point, float weight = 1f ) {
        var currentIndex = controlPoints.IndexOf( point );
        var nextIndex = ( currentIndex - 1 ) % controlPoints.Count;

        if ( nextIndex < 0 ) {
            nextIndex += controlPoints.Count;
        }

        var position = ( controlPoints[nextIndex].position + controlPoints[currentIndex].position ) * 0.5f;

        return InsertControlPoint( nextIndex + 1, position, weight );
    }

    public ControlPoint InsertControlPointAfter( ControlPoint point, float weight = 1f ) {
        var currentIndex = controlPoints.IndexOf( point );
        var nextIndex = ( currentIndex + 1 ) % controlPoints.Count;
        var position = ( controlPoints[nextIndex].position + controlPoints[currentIndex].position ) * 0.5f;

        return InsertControlPoint( nextIndex, position, weight );
    }

    public ControlPoint InsertControlPoint( int index, Vector3 position, float weight = 1f ) {
        var controlPoint = new ControlPoint {
            position = position,
            weight = weight
        };

        controlPoints.Insert( index, controlPoint );

        CreateCurve();

        return controlPoint;
    }

    public void RemoveControlPoint( ControlPoint controlPoint ) {
        controlPoints.Remove( controlPoint );

        Recalculate();
    }

    public void CreateCurve() {

        var normals = new Vector3[controlPoints.Count];
        var points = new Vector3[controlPoints.Count];
        var tangents = new Vector3[controlPoints.Count];
        var weights = new float[controlPoints.Count];

        controlPoints.RemoveAll( that => that == null );

        for ( var i = 0; i < controlPoints.Count; ++i ) {
            var each = controlPoints[i];

            points[i] = each.position;
            tangents[i] = each.tangent;
            weights[i] = each.weight;
            normals[i] = Vector3.up; //( Quaternion.AngleAxis ( i * 20f, Vector3.forward ) * Vector3.up ).normalized;
        }

        curve = new HermiteSpline( degree, points, tangents, normals, weights, isClosed );

        bounds = CalculateBounds();
    }

    private float GetDerivativeAsFloat( float x ) {

        return curve.EvaluateDerivative( x ).magnitude;
    }

    public void Recalculate() {

        CreateCurve();

        var integrator = new AdaptiveSimpsonsMethod();

        Func<float, float, float> arcLengthFunction =
            ( from, to ) => integrator.Evaluate( GetDerivativeAsFloat, from, to );

        parameterToDistanceApproximation = new AdaptiveChebyshevApproximation {
            f = arcLengthFunction,
            from = 0,
            to = 1f,
            isPiecewise = true
        };

        parameterToDistanceApproximation.CalculateApproximation();

        length = integrator.Evaluate( GetDerivativeAsFloat, 0f, 1f );

        //length = parameterToDistanceApproximation.Evaluate ( 1f );

        Func<float, float, float> distanceToParameterFunction =
            ( from, to ) =>
                RootApproximation.Evaluate( t => to * length - parameterToDistanceApproximation.Evaluate( t ) );

        distanceToParameterApproximation = new AdaptiveChebyshevApproximation {
            f = distanceToParameterFunction,
            from = 0,
            to = 1,
            isPiecewise = false
        };

        //var timer = new System.Diagnostics.Stopwatch();
        //timer.Start();

        //for ( int i = 0; i < 100; i++ ) {

        distanceToParameterApproximation.CalculateApproximation();
        //}

        //timer.Stop();
        //Debug.Log( "Execution: " + timer.Elapsed.Ticks / 100 );

    }

    //void Update() {
    //	Recalculate();
    //}

    private float ParameterToDistance( float t ) {

        return parameterToDistanceApproximation.Evaluate( isInverted ? 1f - t : t );
    }

    public float DistanceToParameter( float distance, ref int intervalIndex ) {

        var result = ( distance / length ).Clamped( 0f, 1f );

        return distanceToParameterApproximation.Evaluate( ref intervalIndex, isInverted ? 1f - result : result );
    }

    public float DistanceToParameter( float distance ) {

        var result = ( distance / length ).Clamped( 0f, 1f );

        return distanceToParameterApproximation.Evaluate( isInverted ? 1f - result : result );
    }

    public float CalculateProjectionDistanceAtDistanceRange( Vector3 point, float from, float to ) {

        return CalculateProjectionDistanceAtRange( point, DistanceToParameter( from ), DistanceToParameter( to ) );
    }

    public float CalculateProjectionDistanceAtRange( Vector3 point, float from, float to ) {

        var localPoint = transform.InverseTransformPoint( point );

        var firstApproximation =
            RootApproximation.Evaluate(
                t => Vector3.Dot( localPoint - curve.Evaluate( t ), curve.EvaluateDerivative( t ) ),
                Mathf.Min( from, to ), Mathf.Max( from, to ) );

        var result = parameterToDistanceApproximation.Evaluate( firstApproximation );

        return isInverted ? length - result : result;
    }

    public float CalculateProjectionDistance( Vector3 point ) {

        return CalculateProjectionDistanceAtRange( point, 0f, 1f );
    }

    public Vector3 CalculateProjectionPoint( Vector3 point ) {

        return CalculatePointByDistance( CalculateProjectionDistance( point ) );
    }

    public Quaternion CalculateRotationByDistance( float distance ) {

        return CalculaterRotationByParameter( DistanceToParameter( distance ) );
    }

    private Quaternion CalculaterRotationByParameter( float parameter ) {

        var forward = curve.EvaluateDerivative( parameter ); //Vector3.Scale( , transform.localScale );

        return transform.rotation * Quaternion.LookRotation( forward, CalculateNormalByParameter( parameter ) );
    }

    public Vector3 CalculatePointByDistance( float distance, ref int intervalIndex ) {

        return CalculatePointByParameter( DistanceToParameter( distance, ref intervalIndex ) );
    }

    public Vector3 CalculatePointByDistance( float distance ) {

        return CalculatePointByParameter( DistanceToParameter( distance ) );
    }

    public Vector3 CalculateNormalByParameter( float parameter ) {

        return curve.EvaluateNormal( parameter );
    }

    private Vector3 CalculatePointByParameter( float parameter ) {

        return transform.TransformPoint( curve.Evaluate( parameter ) );
    }

    public void CalculateTransformByDistance( float distance, out Vector3 position, out Quaternion rotation ) {

        var parameter = DistanceToParameter( distance );

        position = CalculatePointByParameter( parameter );
        rotation = CalculaterRotationByParameter( parameter );
    }

    public void CalculateTransformByDistance( float distance, out Vector3 position, out Quaternion rotation,
        ref int pivot ) {

        var parameter = DistanceToParameter( distance );

        position = CalculatePointByParameter( parameter );
        rotation = CalculaterRotationByParameter( parameter );
    }

    public void CalculateTransformByDistance( float distance, Transform transform ) {

        var parameter = DistanceToParameter( distance );

        transform.position = CalculatePointByParameter( parameter );
        transform.rotation = CalculaterRotationByParameter( parameter );
    }

}