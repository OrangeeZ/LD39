using UnityEngine;
using System.Collections;
using System.Linq;

public class SphereLevelGenerator : MonoBehaviour {

    public static event System.Action Completed = delegate {};

    public GameObject[] prefabs;

    private void Start() {

        var localToWorldMatrix = transform.localToWorldMatrix;

        foreach ( var each in GetComponent<MeshFilter>().sharedMesh.vertices.Distinct() ) {

            var worldPos = localToWorldMatrix.MultiplyPoint3x4( each );

            Instantiate( prefabs.RandomElement(), worldPos, Quaternion.FromToRotation( Vector3.up, worldPos ) );
        }
        
        Completed();
    }
}