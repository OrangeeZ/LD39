using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif


public class TestRandomFactory : MonoBehaviour {
	
	public Dictionary<string, float> StringChances = new Dictionary<string, float> ()
	{
		{"1.1", 1.1f},
		{"2.2", 1.1f},
		{"3.3", 1.1f},
		{"4.4", 1.1f},
		{"5.5", 1.1f},
		{"6.6", 1.1f},
		{"7.7", 1.1f},
		{"8.8", 1.1f},
	};
	private RandomFactory<string> _randomString = new RandomFactory<string> ();
	
	
	void Start () {
		_randomString.Initialize (StringChances);
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		_randomString.Get();
		
	}
	

	void Update () {
	
	}
}
