using UnityEngine;
using UnityEditor;
using System.Collections;

public class BurningConfigurator : EditorWindow
{
	[MenuItem("Tools/Burning splitter")]
	public static void ShowMe()
	{
		EditorWindow.GetWindow<BurningConfigurator>().Show();
	}

	Object _burning;
	Object _target;

	void OnGUI()
	{
		_burning = EditorGUILayout.ObjectField("Burning", _burning, typeof(Burning), false);
		_target = EditorGUILayout.ObjectField("Object to split", _target, typeof(MeshRenderer), true);

		if (GUILayout.Button("Split")) {
			Burning burning = (Burning)_burning;
			float size = burning.spreadRange * burning.scaleMultiplier;

			MeshRenderer renderer = (MeshRenderer)_target;
			var bounds = renderer.bounds;
		}
	}
}

