using System;
using System.IO;
using System.Threading;
using CsvHelper;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using csv;

public static class GoogleDocsCsvParser {

	public static void Get( string url, string type, CsvParseRegime regime, string postfix ) {

		EditorUtility.DisplayProgressBar( "Loading", "Requesting csv file. Please wait...", 0f );
		Debug.Log( "Loading csv from: " + url );
		var www = new WWW( url );
		ContinuationManager.Add( () => {
			EditorUtility.DisplayProgressBar( "Loading", "Requesting csv file. Please wait...", www.progress );
			return www.isDone;
		},
			() => {
				EditorUtility.ClearProgressBar();

				// Let's parse this CSV!
				TextReader sr = new StringReader( www.text );
				try {
					if ( regime == CsvParseRegime.ObjectPerRow ) {
						ParseCsv2( sr, type, postfix );
					} else {
						ParseCsv_Sopog( sr, type, postfix );
					}
				}
				catch ( Exception ex ) {
					Debug.LogException( ex );
				}
			} );
	}

	private static void ParseCsv2( TextReader csvReader, string type = null, string postfix = null ) {
		var parser = new CsvParser( csvReader );
		var row = parser.Read(); // get first row and

		if ( string.IsNullOrEmpty( type ) ) {
			// Read Type info
			if ( row[0] == "type" ) {
				type = row[1];

				row = parser.Read();
			} else {
				Debug.LogError( "Worksheet must declare 'Type' in first wor" );
				return;
			}
		}

		// Read fields
		while ( row != null && row[0] != "ID" ) {
			row = parser.Read();
		}
		if ( row == null ) {
			Debug.LogError( "Can't find header!" );
			return;
		}

		var fieldNames = row;
		row = parser.Read();

		while ( row != null ) {
			if ( row.Length < 2 || string.IsNullOrEmpty( row[0] ) ) {
				row = parser.Read();
				continue;
			}

			var instanceName = csv.Utility.FixName( row[0], postfix );
			var instance = GetOrCreate( type, instanceName );
			var csvConfigurable = instance as ICsvConfigurable;

			if ( csvConfigurable != null ) {
				var configurable = csvConfigurable;

				ParseObjectFieldsAndProperties( configurable, CreateValues( fieldNames, row ) );
				ProcessObject( configurable, CreateValues( fieldNames, row ) );
			} else {
				ParseFields2( row, instance, fieldNames );
			}
			Debug.LogFormat( "Data object '{0}' saved to \"{1}\"", instance.name, AssetDatabase.GetAssetPath( instance ) );

			row = parser.Read();
		}
	}

	private static void ParseCsv_Sopog( TextReader csvReader, string type = null, string postfix = null ) {
		var parser = new CsvParser( csvReader );
		var row = parser.Read(); // get first row and

		//if ( string.IsNullOrEmpty( type ) ) {
		//	// Read Type info
		//	if ( row[0] == "type" ) {
		//		type = row[1];

		//		row = parser.Read();
		//	} else {
		//		Debug.LogError( "Worksheet must declare 'Type' in first wor" );
		//		return;
		//	}
		//}

		// Read fields
		//while ( row != null && row[0] != "ID" ) {
		//	row = parser.Read();
		//}
		//if ( row == null ) {
		//	Debug.LogError( "Can't find header!" );
		//	return;
		//}

		var instanceName = csv.Utility.FixName( type, postfix );
		var instance = GetOrCreate( type, instanceName );

		var csvConfigurable = instance as ICsvConfigurable;

		if ( csvConfigurable != null ) {

			var configurable = csvConfigurable;

			ProcessObject( configurable, CreateValues( parser ) );

			Debug.LogFormat( "Data object '{0}' saved to \"{1}\"", instance.name, AssetDatabase.GetAssetPath( instance ) );
		} else {
			Debug.LogError( "This Csv parse mode can't work with not ICsvConfigurable objects!" );
		}

	}

	private static Values CreateValues( CsvParser parser ) {
		var dict = new Dictionary<string, string>();
		var row = parser.Read();

		while ( row != null ) {
			if ( row.Length < 2 || string.IsNullOrEmpty( row[0] ) ) {
				row = parser.Read();
				continue;
			}

			if ( row.Length > 2 ) {
				Debug.LogError( "This Csv parse regime can't parse rows with more than 2 columns!" );
				break;
			}

			//dict[row[0].TrimEnd( ' ' )] = row[1];
			dict.Add( row[0].TrimEnd( ' ' ), row[1] );

			row = parser.Read();
		}
		return new Values( dict );
	}

	private static Values CreateValues( string[] fieldNames, string[] row ) {
		var dict = new Dictionary<string, string>();

		for ( var i = 1; i < row.Length; i++ ) {

			if ( dict.ContainsKey( fieldNames[i] ) ) {

				Debug.LogFormat( "They key is duplicate: {0}:{1}", fieldNames[i], row[i] );
				continue;
			}

			var lowerRow = row[i].ToLower();
			if ( lowerRow == "yes" ) row[i] = "true";
			if ( lowerRow == "no" ) row[i] = "false";

			dict[fieldNames[i].TrimEnd( ' ' )] = row[i];
		}

		return new Values( dict );
	}

	private static void ParseFields2( string[] row, ScriptableObject target, string[] fieldNames ) {
		var type = target.GetType();

		for ( var i = 1; i < row.Length; i++ ) {
			var fieldName = fieldNames[i];
			var fieldValue = row[i];

			var field = type.GetField( fieldName );
			if ( field == null ) {
				Debug.LogErrorFormat( "Type {0} doesn't contains field {1}", type.Name, fieldName );
				return;
			}
			try {
				field.SetValue( target, Convert.ChangeType( fieldValue, field.FieldType ) );
			}
			catch ( Exception ex ) {
				Debug.LogErrorFormat( "Can't set value {0} of field '{1}' to object '{1}'", fieldValue, fieldName, target.name, target );
				return;
			}
		}
	}

	private static ScriptableObject GetOrCreate( string typeName, string instanceName ) {
		var assembly = Assembly.GetAssembly( typeof ( ICsvConfigurable ) );
		var type = assembly.GetExportedTypes().First( ( x ) => x.FullName.Equals( typeName, StringComparison.InvariantCultureIgnoreCase ) );
		if ( type == null ) {
			Debug.LogWarningFormat( "Type {0} not found", typeName );
			return null;
		}

		var assetPath = Path.Combine( "Assets/Data/Remote Data/", type.Name );
		var assetPathWithName = assetPath + "/" + instanceName + ".asset";

		var instance = AssetDatabase.LoadAssetAtPath<ScriptableObject>( assetPathWithName );

		if ( instance == null ) {
			instance = ScriptableObject.CreateInstance( type );
			Directory.CreateDirectory( assetPath ).Attributes = FileAttributes.Normal;
			AssetDatabase.CreateAsset( instance, assetPathWithName );
		}

		EditorUtility.SetDirty( instance );

		return instance;
	}

	private static void ProcessObject( ICsvConfigurable target, Values values ) {

		ParseObjectFieldsAndProperties( target, values );
		target.Configure( values );
	}

	private static void ParseObjectFieldsAndProperties( ICsvConfigurable target, Values values ) {

		var type = target.GetType();

		var fields = type.GetFields( BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance );

		foreach ( var each in fields ) {

			ParseObjectField( target, each, values );
		}
	}

	private static void ParseObjectField( ICsvConfigurable target, FieldInfo fieldInfo, Values values ) {

		var attribute = fieldInfo.GetCustomAttributes( typeof ( RemotePropertyAttribute ), inherit: false ).OfType<RemotePropertyAttribute>().FirstOrDefault();

		if ( attribute != null ) {

			var value = values.Get<string>( attribute.PropertyName, string.Empty );

			var fieldValue = Convert.ChangeType( value, fieldInfo.FieldType );

			fieldInfo.SetValue( target, fieldValue );
		}
	}

}