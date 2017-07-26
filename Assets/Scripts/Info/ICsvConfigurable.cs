using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace csv {

	public static class Utility {

		public static string FixName( string name, string postfix = null ) {

			var builder = new StringBuilder();

			var wordStart = -1;
			var wordEnd = -1;
			var pushed = 0;
			// Convert to lower
			for ( var i = 0; i < name.Length; i++ ) {
				if ( char.IsUpper( name, i ) ) {
					if ( wordStart != -1 ) {
						if ( wordEnd != wordStart ) {
							// New word
							if ( builder.Length > 0 ) {
								builder.Append( "-" );
							}
							builder.Append( name.Substring( pushed, i - wordStart ).ToLower() );
							pushed = i;
						}
					} else if ( pushed < i ) {
						if ( builder.Length > 0 ) {
							builder.Append( "-" );
						}
						builder.Append( name.Substring( pushed, i ).ToLower() );
						pushed = i;
					}
					wordStart = i;
					wordEnd = i;
				} else {
					if ( wordStart != -1 ) {
						wordEnd = i;
					}
				}
			}

			if ( pushed < name.Length - 1 ) {
				if ( builder.Length > 0 ) {
					builder.Append( "-" );
				}
				builder.Append( name.Substring( pushed ).ToLower() );
			}

			var result = builder.ToString();
			if ( !string.IsNullOrEmpty( postfix ) ) {
				result += "-" + postfix;
			}
			return result;
		}

	}

	public class Values {

		private Dictionary<string, string> _values;

		public Values( Dictionary<string, string> values ) {
			_values = values;
		}

		public Dictionary<string, string> Raw {
			get { return _values; }
		}

		public void Get<T>( string name, out T value ) {

			value = Get<T>( name );
		}

		public void GetEnum<T>( string name, out T value ) {

			var stringValue = Get<string>( name );

			value = (T)Enum.Parse( typeof ( T ), stringValue );
		}

		public T Get<T>( string name ) {
			return Get( name, default ( T ) );
		}

		public T Get<T>( string name, T defaultValue ) {
			string strValue;
			if ( _values.TryGetValue( name, out strValue ) ) {
				return As<T>( strValue, defaultValue );
			}
			return defaultValue;
		}

		public T As<T>( string strValue, T defaultValue ) {
			try {
				return (T) System.Convert.ChangeType( strValue, typeof ( T ) );
			}
			catch ( System.Exception ex ) {
				return default ( T );
			}
		}

		public T[] GetScriptableObjects<T>( string name ) where T : ScriptableObject {

			var names = Get( name, string.Empty ).Split( ',', ' ' );

#if UNITY_EDITOR
			return names.Select( _ => LoadScriptableObject<T>( _ ) ).Where( _ => _ != null ).ToArray();
#endif
			return null;
		}

		public T GetScriptableObject<T>( string name ) where T : ScriptableObject {

			var assetName = Get( name, string.Empty );

			return LoadScriptableObject<T>( assetName );
		}

		public T GetPrefabWithComponent<T>( string name, bool fixName ) where T : Component {

			var assetName = Get( name, string.Empty );

			if ( fixName ) {

				assetName = Utility.FixName( assetName );
			}

#if UNITY_EDITOR
			var guids = AssetDatabase.FindAssets( "t: prefab " + assetName );
			var paths = guids.Select( _ => AssetDatabase.GUIDToAssetPath( _ ) );

			return paths.Select( _ => AssetDatabase.LoadAssetAtPath<T>( _ ) ).FirstOrDefault();
#endif
			return null;
		}

		private T LoadScriptableObject<T>( string name ) where T : ScriptableObject {

			name = Utility.FixName( name );

			if ( name.IsNullOrEmpty() ) {

				return null;
			}

#if UNITY_EDITOR
			var foundObjects = AssetDatabase.FindAssets( "t:" + typeof ( T ).Name + " " + name ).Select( _ => AssetDatabase.GUIDToAssetPath( _ ) ).Select( _ => AssetDatabase.LoadAssetAtPath<T>( _ ) );
			//if ( guid != null ) {

			//	var path = AssetDatabase.GUIDToAssetPath( guid );
			//	return AssetDatabase.LoadAssetAtPath<T>( path );
			//}

			return foundObjects.FirstOrDefault( where => where.name == name );

			//Debug.LogFormat( "Could not find {0}", "t:" + typeof ( T ).Name + " " + name );
#endif

			return null;
		}

	}

}

public interface ICsvConfigurable {

	void Configure( csv.Values values );

}