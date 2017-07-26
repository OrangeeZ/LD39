using UnityEngine;

#if UNITY_EDITOR
	using UnityEditor;
	using System.Collections;

	public static class GUIHelper {
		public static void MethodOnButton( string buttonName, System.Action method ) {
			if ( GUILayout.Button( buttonName ) )
				method();
		}
	}
#endif