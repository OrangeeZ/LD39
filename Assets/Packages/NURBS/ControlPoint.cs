using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ControlPoint : AObject {
	public float weight = 1f;
	public ComplexNURBS curve = null;

	public float gizmoRadius = 10f;

#if UNITY_EDITOR
	[SerializeField]
	private Vector3 oldPosition = Vector3.zero;
#endif

	public static ControlPoint Create(ComplexNURBS curve, Vector3 position, float weight){
		var go = new GameObject("ControlPoint");
		go.transform.position = position;
		go.transform.parent = curve.transform;
		
		var controlPoint = go.AddComponent<ControlPoint>();
		controlPoint.weight = weight;
		controlPoint.curve = curve;
		
		return controlPoint;
	}

#if UNITY_EDITOR

	void Awake() {
		oldPosition = localPosition;
	}

	void OnDrawGizmosSelected(){
		if (localPosition != oldPosition)
			curve.CreateCurve();

		oldPosition = localPosition;
	}

#endif

	void OnDrawGizmos(){
        Gizmos.color = Color.green - new Color { a = 0.5f };
		Gizmos.DrawSphere(transform.position, gizmoRadius);	
	}
}
