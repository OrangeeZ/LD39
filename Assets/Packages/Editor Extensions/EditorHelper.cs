using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Collections;

public static class EditorHelper {
	private static TextureImporter GetTextureImporter( Texture2D texture ) {
		var assetPath = AssetDatabase.GetAssetPath( texture );
		return AssetImporter.GetAtPath( assetPath ) as TextureImporter;
	}

	public static void MakeTextureReadable( Texture2D texture ) {
		var assetPath = AssetDatabase.GetAssetPath( texture );
		var textureImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;

		if ( !textureImporter.isReadable ) {
			textureImporter.isReadable = true;
			AssetDatabase.ImportAsset( assetPath );
		}
	}

	public static void SetTexureFormat( Texture2D texture, TextureImporterFormat format ) {
		var assetPath = AssetDatabase.GetAssetPath( texture );
		var textureImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;

		textureImporter.textureFormat = format;

		AssetDatabase.ImportAsset( assetPath );
	}

	public static TextureImporterFormat GetTexureFormat( Texture2D texture ) {
		var assetPath = AssetDatabase.GetAssetPath( texture );
		var textureImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;

		return textureImporter.textureFormat;
	}

	public static Texture2D ObjectField( Texture2D sourceObject ) {
		return EditorGUILayout.ObjectField( sourceObject, typeof( Texture2D ), false, GUILayout.Width( 60 ), GUILayout.Height( 60 ) ) as Texture2D;
	}

	public static Texture2D ObjectField( string label, Texture2D sourceObject ) {
		var characterWidth = 8;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( label, GUILayout.Width( label.Length * characterWidth ) );

		var result = ObjectField( sourceObject );
		EditorGUILayout.EndHorizontal();

		return result;
	}

	public static T ObjectField<T>( T sourceObject ) where T : Object {
		return EditorGUILayout.ObjectField( sourceObject, typeof( T ), false ) as T;
	}

	public static T ObjectField<T>( string label, T sourceObject ) where T : Object {
		var characterWidth = 8;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( label, GUILayout.Width( label.Length * characterWidth ) );

		var result = ObjectField( sourceObject );
		EditorGUILayout.EndHorizontal();

		return result;
	}

	public static void ObjectField<T>( string label, ref T sourceObject ) where T : Object {
		var characterWidth = 8;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField( label, GUILayout.Width( label.Length * characterWidth ) );

		sourceObject = ObjectField( sourceObject );
		EditorGUILayout.EndHorizontal();
	}
}

#endif