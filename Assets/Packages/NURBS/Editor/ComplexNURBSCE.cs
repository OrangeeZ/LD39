using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

[CustomEditor( typeof ( ComplexNURBS ) )]
public class ComplexNURBSCE : Editor<ComplexNURBS> {

    private static Texture2D splineTexture = null;
    private static Texture2D tangentTexture = null;
    private int selectedControlPoint = -1;

    private Tool lastTool = Tool.None;

    private int insertionIndex = 0;

    private bool showControlPoints = false;
    private bool showControlPointOptions = false;

    private int controlPointRadius = 8;
    private int controlPointPickRadius = 8;

    [MenuItem( "Addons/Create NURBS" )]
    private static void Create() {

        var instance = new GameObject( "NURBS" ).AddComponent<ComplexNURBS>();

        instance.AddControlPoint( Vector3.zero );
        instance.InsertControlPoint( 1, Vector3.right * 5f );

        instance.Recalculate();
    }

    private void RebuildCurveTexture() {

        DestroyImmediate( splineTexture );

        splineTexture = new Texture2D( 1, 8, TextureFormat.RGB24, mipmap: false, linear: true );

        splineTexture.SetPixels32( new Color32[] {
            Color.clear, Color.black, target.curveColor, target.curveColor, target.curveColor, target.curveColor,
            Color.black, Color.clear
        } );
        splineTexture.Apply();
    }

    private void OnEnable() {

        RebuildCurveTexture();

        if ( tangentTexture == null ) {

            tangentTexture = new Texture2D( 1, 7, TextureFormat.RGB24, mipmap: false, linear: true );

            tangentTexture.SetPixels32( new Color32[]
            {Color.clear, Color.black, Color.white, Color.white, Color.white, Color.black, Color.clear} );
            tangentTexture.Apply();
        }

        lastTool = Tools.current;
        Tools.current = Tool.None;

        SceneView.onSceneGUIDelegate += OnSceneGUIDelegate;
    }

    private void OnDisable() {

        Tools.current = lastTool;

        SceneView.onSceneGUIDelegate -= OnSceneGUIDelegate;
    }

    private void OnDestroy() {

        SceneView.onSceneGUIDelegate -= OnSceneGUIDelegate;
    }

    public override void OnInspectorGUI() {

        target.degree = EditorGUILayout.IntField( "Curve degree", target.degree );
        target.isClosed = EditorGUILayout.Toggle( "Is Closed", target.isClosed );
        target.isInverted = EditorGUILayout.Toggle( "Is Inverted", target.isInverted );
        target.drawDottedLine = EditorGUILayout.Toggle( "Is Dotted", target.drawDottedLine );

        var newCurveColor = EditorGUILayout.ColorField( "Curve color", target.curveColor );

        if ( target.curveColor != newCurveColor ) {

            RebuildCurveTexture();
        }

        target.curveColor = newCurveColor;

        if ( GUILayout.Button( "Add point" ) ) {

            Undo.RecordObject( target, "Add control point" );

            if ( target.controlPoints.Count > 1 ) {

                target.AddControlPoint( target.CalculatePointByDistance( target.length ) +
                                        target.CalculateRotationByDistance( target.length ) * Vector3.forward );
            }
            else {

                target.AddControlPoint( Vector3.zero );
            }

            selectedControlPoint = target.controlPoints.Count - 1;

            SceneView.RepaintAll();
        }

        GUILayout.BeginHorizontal();

        insertionIndex = EditorGUILayout.IntField( "Insertion at", insertionIndex );
        GUIHelper.MethodOnButton( "Insert", () => {

            Undo.RecordObject( target, "Insert control point" );

            target.InsertControlPoint( insertionIndex, target.controlPoints[insertionIndex].position - Vector3.right );
            selectedControlPoint = insertionIndex;
        } );

        GUILayout.EndHorizontal();

        showControlPoints = EditorGUILayout.Foldout( showControlPoints, "Control points" );

        if ( showControlPoints ) {

            var i = 0;
            foreach ( var each in target.controlPoints ) {

                each.showInInspector = EditorGUILayout.Foldout( each.showInInspector, "Point #" + ++i );

                if ( each.showInInspector ) {

                    GUILayout.BeginHorizontal();

                    if ( GUILayout.Button( "Copy" ) ) {

                        EditorGUIUtility.systemCopyBuffer = each.ToString();
                    }

                    if ( GUILayout.Button( "Paste" ) ) {

                        each.FromString( EditorGUIUtility.systemCopyBuffer );

                        target.Recalculate();
                    }

                    GUILayout.EndHorizontal();

                    each.position = EditorGUILayout.Vector3Field( "Position", each.position );
                    each.tangent = EditorGUILayout.Vector3Field( "Tangent", each.tangent );
                }
            }
        }

        if ( selectedControlPoint != -1 ) {

            var currentControlPoint = target.controlPoints[selectedControlPoint];

            showControlPointOptions = EditorGUILayout.Foldout( showControlPointOptions, "Selected control point" );

            if ( showControlPointOptions ) {

                currentControlPoint.position = EditorGUILayout.Vector3Field( "Position", currentControlPoint.position );
                currentControlPoint.tangent = EditorGUILayout.Vector3Field( "Tangent", currentControlPoint.tangent );

                SceneView.RepaintAll();
            }
        }

        if ( GUILayout.Button( "Stitch first control point with last" ) ) {

            if ( target.controlPoints.Count > 0 ) {

                Undo.RecordObject( target, "Stitch first control point with last" );

                target.controlPoints[0] = target.controlPoints[target.controlPoints.Count - 1];

                target.Recalculate();
            }

        }

        if ( GUILayout.Button( "Stitch last control point with first" ) ) {

            if ( target.controlPoints.Count > 0 ) {

                Undo.RecordObject( target, "Stitch last control point with first" );

                target.controlPoints[target.controlPoints.Count - 1] = target.controlPoints[0];

                target.Recalculate();
            }
        }

        if ( GUILayout.Button( "Unstitch first from last" ) ) {

            if ( target.controlPoints.Count > 0 ) {

                Undo.RecordObject( target, "Unstitch first from last" );

                target.controlPoints[0] = new ComplexNURBS.ControlPoint();
                target.controlPoints[0].Copy( target.controlPoints[target.controlPoints.Count - 1] );

                target.Recalculate();
            }
        }

        if ( GUILayout.Button( "Recalculate" ) ) {

            target.Recalculate();
        }
    }

    private void OnSceneGUIDelegate( SceneView currentSceneView ) {

        if ( !target.drawDottedLine ) {

            DrawCurve();
        }
        else {

            DrawDottedCurve();
        }

        var camera = currentSceneView.camera;
        var controlPoints = target.controlPoints;

        if ( Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete ) {

            if ( selectedControlPoint != -1 ) {

                Undo.RecordObject( target, "Remove control point" );

                target.RemoveControlPoint( controlPoints[selectedControlPoint] );

                selectedControlPoint = -1;

                Event.current.Use();

                return;
            }
        }

        var transformationMatrix = target.transform.localToWorldMatrix;
        var inverseTransformationMatrix = transformationMatrix.inverse;

        //Handles.BeginGUI();

        //var pointPixelSize = 12;

        for ( var i = 0; i < controlPoints.Count; ++i ) {

            var each = controlPoints[i];
            var worldPosition = ( transformationMatrix.MultiplyPoint3x4( each.position ) );
            var handleSize = HandleUtility.GetHandleSize( worldPosition );

            if ( Handles.Button( worldPosition, camera.transform.rotation, handleSize / controlPointRadius,
                handleSize / controlPointPickRadius, DrawTexturedCircle ) ) {

                selectedControlPoint = i;

                Repaint();

                break;
            }
        }

        //Handles.EndGUI();

        if ( selectedControlPoint != -1 ) {

            var currentControlPoint = controlPoints[selectedControlPoint];

            var oldPosition = transformationMatrix.MultiplyPoint3x4( currentControlPoint.position );
            var newPosition = Handles.PositionHandle( oldPosition, Quaternion.identity );

            if ( newPosition != oldPosition ) {

                Undo.RecordObject( target, "Change control point position" );

                currentControlPoint.position = inverseTransformationMatrix.MultiplyPoint3x4( newPosition );

                target.Recalculate();
            }

            var oldTangent =
                transformationMatrix.MultiplyPoint3x4( currentControlPoint.position + currentControlPoint.tangent );
            var newTangent = Handles.PositionHandle( oldTangent, Quaternion.identity );
                //, Quaternion.identity, HandleUtility.GetHandleSize( newPosition ), snap: 0f );

            if ( newTangent != oldTangent ) {

                Undo.RecordObject( target, "Change control point tangent" );

                currentControlPoint.tangent = inverseTransformationMatrix.MultiplyPoint3x4( newTangent ) -
                                              currentControlPoint.position;

                target.Recalculate();
            }

            var handleSize = HandleUtility.GetHandleSize( newPosition );

            DrawTangent( newPosition, newTangent, camera.transform.rotation, handleSize / controlPointRadius * 0.8f );
            DrawTangent( newPosition, 2f * newPosition - newTangent, camera.transform.rotation,
                handleSize / controlPointRadius * 0.8f );

            //Handles.ArrowCap( 0, newPosition, target.rotation * Quaternion.FromToRotation( Vector3.forward, currentControlPoint.tangent ), currentControlPoint.tangent.magnitude );

            //Handles.BeginGUI();
            //for ( var i = 0; i < controlPoints.Count; ++i ) {
            //	var each = controlPoints[i];
            //	var screenPosition = camera.WorldToScreenPoint( each.position + target.position + each.tangent );

            //	if ( GUI.Button( new Rect( screenPosition.x - 10, SceneView.currentDrawingSceneView.camera.pixelRect.height - screenPosition.y - 10, 20, 20 ), splineTexture ) ) {
            //		selectedControlPoint = i;

            //		each.tangent += camera.ScreenToWorldPoint( Event.current.delta );

            //		break;
            //	}
            //}
            //Handles.EndGUI();
        }
    }

    private void DrawCurve( float step = 0.25f ) {

        if ( target.controlPoints.IsNullOrEmpty() ) {

            return;
        }

        var points = new Vector3[(int) ( target.length / step ) + 2];

        for ( var i = 0; i < points.Length; ++i ) {

            points[i] = target.CalculatePointByDistance( i * step );
        }

        Handles.DrawAAPolyLine( splineTexture, points );

    }

    private void DrawDottedCurve( float step = 0.25f ) {

        for ( var i = 0f; i < target.length; i += step ) {

            Handles.DrawAAPolyLine( splineTexture,
                new[] {target.CalculatePointByDistance( i ), target.CalculatePointByDistance( i + step * 0.5f )} );
        }
    }

    private static void DrawTexturedCircle( int id, Vector3 position, Quaternion rotation, float radius ) {

        Handles.DrawSolidDisc( position, rotation * Vector3.forward, radius );
    }

    private static void DrawTangent( Vector3 from, Vector3 to, Quaternion cameraRotation, float radius ) {

        Handles.DrawAAPolyLine( tangentTexture, from, to );

        DrawTexturedCircle( -1, to, cameraRotation, radius );
    }

}