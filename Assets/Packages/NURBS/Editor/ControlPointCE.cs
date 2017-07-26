using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor( typeof( ControlPoint ) )]
[CanEditMultipleObjects]
public class ControlPointCE : Editor<ControlPoint> {
	public override void OnInspectorGUI () {

        serializedObject.Update();

        var gizmoRadiusProperty = serializedObject.FindProperty( "gizmoRadius" );
        gizmoRadiusProperty.floatValue = EditorGUILayout.Slider( gizmoRadiusProperty.floatValue, 0.1f, 10f );

        serializedObject.ApplyModifiedProperties();
	}
}
