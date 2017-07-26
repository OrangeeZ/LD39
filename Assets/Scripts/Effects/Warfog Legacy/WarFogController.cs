using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFogController : MonoBehaviour {

	public static List<WarFogTracer> Tracers = new List<WarFogTracer>();
	
	[SerializeField]
	private WarFogSpaceMap _warFogSpaceMap;
	
	void Update() {

		foreach ( var each in Tracers ) {

			each.Trace( _warFogSpaceMap );
		}

		_warFogSpaceMap.SubmitTexture();

		GetComponentInChildren<MeshRenderer>().material.mainTexture = _warFogSpaceMap.GetTexture();
	}
}
