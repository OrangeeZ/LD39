using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class AssetHelper {

	private static IEnumerable<string> GetFilesInFolder( string folder ) {
		var files = new List<string>();

		foreach ( var dir in Directory.GetDirectories( folder ) ) {
			files.AddRange( GetFilesInFolder( dir ) );
		}

		files.AddRange( Directory.GetFiles( folder ).Select( file => file.Replace( Application.dataPath, "Assets" ) ) );

		return files;
	}

	public static IEnumerable<T> GetAllAssetsOfType<T>() where T : Object {

		var guids = AssetDatabase.FindAssets( "t: " + typeof ( T ).Name );
		var paths = guids.Select( _ => AssetDatabase.GUIDToAssetPath( _ ) );

		return paths.Select( _ => AssetDatabase.LoadAssetAtPath<T>( _ ) );
	}

	public static IEnumerable<T> GetAllPrefabsWithBehaviour<T>() where T : Behaviour {

		return AssetDatabase.GetAllAssetPaths()
			.Where( _ => _.Contains( ".prefab" ) )
			.Select( _ => AssetDatabase.LoadAssetAtPath( _, typeof( GameObject ) ) as GameObject )
			.Where( _ => _ != null )
			.Select( _ => _.GetComponentsInChildren<T>( includeInactive: true ).FirstOrDefault() )
			.Where( _ => _ != null );
	}

	public static IEnumerable<T> GetAllPrefabsWithBehaviours<T>() where T : Behaviour {

		return AssetDatabase.GetAllAssetPaths()
			.Where( _ => _.Contains( ".prefab" ) )
			.Select( _ => AssetDatabase.LoadAssetAtPath( _, typeof( Transform ) ) )
			.OfType<Behaviour>()
			.SelectMany( _ => _.GetComponentsInChildren<T>( includeInactive: true ) )
			.Where( _ => _ != null );
	}

	public static IEnumerable<T> GetAllAssetsOfType<T>( string extension ) where T : Object {

		Resources.UnloadUnusedAssets();

		return AssetDatabase.GetAllAssetPaths().Where( each => each.Contains( extension ) ).Select<string, Object>( AssetDatabase.LoadMainAssetAtPath ).OfType<T>();
	}

	public static T CreateAsset<T>() where T : ScriptableObject {

		var path = AssetDatabase.GetAssetPath( Selection.activeObject );
		if ( path == "" ) {
			path = "Assets";
		} else if ( Path.GetExtension( path ) != "" ) {
			path = path.Replace( Path.GetFileName( AssetDatabase.GetAssetPath( Selection.activeObject ) ), "" );
		}

		return CreateAsset<T>( path );
	}

	public static T CreateAsset<T>( string path ) where T : ScriptableObject {

		var asset = ScriptableObject.CreateInstance<T>();

		var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath( path + "/New " + typeof( T ) + ".asset" );

		AssetDatabase.CreateAsset( asset, assetPathAndName );

		AssetDatabase.SaveAssets();
		Selection.activeObject = asset;

		return asset;
	}
}

#endif