using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomEditor( typeof( RemoteDataLoader ) )]
public class RemoteDalaLoaderEditor : Editor {

	private SerializedProperty _url;
	private SerializedProperty _pageId;
	private SerializedProperty _type;
	private SerializedProperty _postfix;
	private SerializedProperty _regime;

	private int _selectedType;
	private string[] _types;

	private void OnEnable() {
		_url = serializedObject.FindProperty( "_url" );
		_pageId = serializedObject.FindProperty( "_pageId" );
		_type = serializedObject.FindProperty( "type" );
		_postfix = serializedObject.FindProperty( "postfix" );
		_regime = serializedObject.FindProperty( "_regime" );

		var types = typeof( ICsvConfigurable ).Assembly.GetExportedTypes();
		_types = types.Where( t => t.IsSubclassOf( typeof( ScriptableObject ) ) ).Select( t => t.FullName ).ToArray();
		_selectedType = ArrayUtility.IndexOf( _types, _type.stringValue );
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		EditorGUILayout.PropertyField( _url );
		EditorGUILayout.PropertyField( _pageId );
		EditorGUILayout.PropertyField( _postfix );
		EditorGUILayout.PropertyField( _regime );

		EditorGUILayout.PropertyField( _type );

		var selected = EditorGUILayout.Popup( _selectedType, _types );
		if ( selected != _selectedType ) {
			_selectedType = selected;
			if ( _selectedType != -1 ) {
				_type.stringValue = _types[_selectedType];
			}
		}

		//base.OnInspectorGUI ();
		serializedObject.ApplyModifiedProperties();

		if ( GUILayout.Button( "Load" ) ) {
			( (RemoteDataLoader) target ).LoadRemoteData();
		}
	}

}