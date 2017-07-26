using UnityEngine;

#if UNITY_EDITOR
	using UnityEditor;
	using System.Collections;

	public class Editor<T> : UnityEditor.Editor where T : class{
		new protected T target {
			get {
				return base.target as T;
			}
		}
	}
#endif